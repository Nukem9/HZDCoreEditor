#include <regex>

#include "HRZ/Core/GGRTTI.h"
#include "HRZ/Core/ExportedSymbol.h"

#include "common.h"
#include "MSRTTI.h"
#include "RTTIIDAExporter.h"

extern std::unordered_set<const GGRTTI *> AllRegisteredTypeInfo;

namespace RTTIIDAExporter
{
	void ExportAll(const char *Directory)
	{
		CreateDirectoryA(Directory, nullptr);

		char outputFilePath[MAX_PATH];
		sprintf_s(outputFilePath, "%s\\IDA_%s_Typeinfo.idc", Directory, g_GamePrefix);

		if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
		{
			fprintf(f, "#include <idc.idc>\n\n");
			fprintf(f, "static main()\n{\n");

			ExportMSRTTI(f);
			ExportGGRTTI(f);
			ExportGameSymbolRTTI(f);

			fprintf(f, "}\n");
			fclose(f);
		}
	}

	void ExportMSRTTI(FILE *F)
	{
		// Order by most complex inheritance to least complex
		auto allRTTI = MSRTTI::FindAll();

		std::sort(allRTTI.begin(), allRTTI.end(), [](const MSRTTI::Info *A, const MSRTTI::Info *B)
		{
			return A->Locator->ClassDescriptor.Get()->NumBaseClasses > B->Locator->ClassDescriptor.Get()->NumBaseClasses;
		});

		for (auto& rtti : allRTTI)
		{
			// Shitty function to create valid names
			std::string className = std::regex_replace(rtti->Name, std::regex("__ptr64|protected: |private: |public: |struct |class |\\.| |\\?|\\*|&|'|\\(|\\)|-"), "");

			for (char& c : className)
			{
				switch (c)
				{
				case ',':
				case '<':
				case '>':
				case '`':
					c = '_';
					break;
				}
			}

			for (uint64_t i = 0; i < rtti->VFunctionCount; i++)
			{
				uintptr_t vfuncPointer = rtti->VTableAddress + (i * sizeof(uintptr_t));
				uintptr_t vfunc = *reinterpret_cast<uintptr_t *>(vfuncPointer);
				uintptr_t vfuncDBAddress = vfunc - g_ModuleBase + 0x140000000;

				// Make sure it's not a pure virtual function
				if (*reinterpret_cast<uint8_t *>(vfunc + 0) == 0xFF && *reinterpret_cast<uint8_t *>(vfunc + 1) == 0x25)
					fprintf(F, "set_name(0x%llX, \"_purecall\");\n", vfuncDBAddress);
				else
					fprintf(F, "set_name(0x%llX, \"%s::VFunc%02lld_%llX\");\n", vfuncDBAddress, className.c_str(), i, vfuncDBAddress);
			}
		}
	}

	void ExportGGRTTI(FILE *F)
	{
		for (auto& type : AllRegisteredTypeInfo)
		{
			auto symbolName = type->GetSymbolName();
			auto printDBOffset = [F](const void *Pointer, const char *Format, const char *Name)
			{
				if (!Pointer)
					return;

				uintptr_t address = reinterpret_cast<uintptr_t>(Pointer) - g_ModuleBase + 0x140000000;
				fprintf(F, Format, address, Name, address);
			};

			printDBOffset(type, "set_name(0x%llX, \"GGRTTI_%s_%llX\");", symbolName.c_str());
			fprintf(F, "// 0x%llX\n", type->GetCoreBinaryTypeId());

			if (auto asClass = type->AsClass(); asClass)
			{
				for (auto& event : asClass->ClassEventSubscriptions())
				{
					auto formatStr = symbolName + "::" + event.m_Type->GetSymbolName();
					printDBOffset(event.m_Callback, "set_name(0x%llX, \"%sCallback_%llX\");\n", formatStr.c_str());
				}

				for (auto& member : asClass->ClassMembers())
				{
					if (!member.m_Type)
						continue;

					auto formatStr = symbolName + "::" + std::string(member.m_Name);
					printDBOffset(member.m_PropertyGetter, "set_name(0x%llX, \"%sGetter_%llX\");\n", formatStr.c_str());
					printDBOffset(member.m_PropertySetter, "set_name(0x%llX, \"%sSetter_%llX\");\n", formatStr.c_str());
				}

				printDBOffset(asClass->m_Constructor, "set_name(0x%llX, \"%s::RTTIConstructor_%llX\");\n", symbolName.c_str());
				printDBOffset(asClass->m_Destructor, "set_name(0x%llX, \"%s::RTTIDestructor_%llX\");\n", symbolName.c_str());
				printDBOffset(asClass->m_Deserialize, "set_name(0x%llX, \"%s::RTTIDeserializeText_%llX\");\n", symbolName.c_str());
				printDBOffset(asClass->m_Serialize, "set_name(0x%llX, \"%s::RTTISerializeText_%llX\");\n", symbolName.c_str());
			}
		}

		fprintf(F, "/*\n");

		// Enum and class members
		for (auto typeInfo : AllRegisteredTypeInfo)
		{
			if (auto asEnum = typeInfo->AsEnum(); asEnum)
			{
				fprintf(F, "// Binary type = 0x%llX\n", asEnum->GetCoreBinaryTypeId());
				fprintf(F, "// sizeof() = 0x%X\n", asEnum->m_EnumUnderlyingTypeSize);
				fprintf(F, "enum %s\n{\n", asEnum->GetSymbolName().c_str());

				for (auto& member : asEnum->EnumMembers())
					fprintf(F, "\t%s = %d,\n", member.m_Name, member.m_Value);

				fprintf(F, "};\n\n");
			}
			else if (auto asClass = typeInfo->AsClass(); asClass)
			{
				auto inheritanceInfo = asClass->ClassInheritance();
				char inheritanceDecl[1024] = {};

				fprintf(F, "// Binary type = 0x%llX\n", asClass->GetCoreBinaryTypeId());
				fprintf(F, "// sizeof() = 0x%X\n", asClass->m_Size);

				if (!inheritanceInfo.empty())
				{
					strcat_s(inheritanceDecl, " : ");

					for (auto inherited : inheritanceInfo)
					{
						strcat_s(inheritanceDecl, inherited.m_Type->GetSymbolName().c_str());
						strcat_s(inheritanceDecl, ", ");

						fprintf(F, "// Base: %s = 0x%X\n", inherited.m_Type->GetSymbolName().c_str(), inherited.m_Offset);
					}

					*strrchr(inheritanceDecl, ',') = '\0';
				}

				for (auto& event : asClass->ClassEventSubscriptions())
					fprintf(F, "// Event callback: %s = 0x%llX\n", event.m_Type->GetSymbolName().c_str(), reinterpret_cast<uintptr_t>(event.m_Callback) - g_ModuleBase + 0x140000000);

				fprintf(F, "class %s%s\n{\npublic:\n", asClass->GetSymbolName().c_str(), inheritanceDecl);

				// Dump all class members
				auto members = asClass->GetCategorizedClassMembers();

				for (auto& [member, category, _] : members)
				{
					// Regular data member
					fprintf(F, "\t%s %s;// 0x%X", member->m_Type->GetSymbolName().c_str(), member->m_Name, member->m_Offset);

					if (member->m_PropertyGetter || member->m_PropertySetter)
						fprintf(F, " (Property)");

					if (member->IsSaveStateOnly())
						fprintf(F, " (SAVE_STATE_ONLY)");

					fprintf(F, "\n");
				}

				fprintf(F, "};\n\n");
			}
		}

		fprintf(F, "*/\n");
	}

	void ExportGameSymbolRTTI(FILE *F)
	{
		auto& gameSymbolGroups = *reinterpret_cast<Array<ExportedSymbolGroup *> *>(g_OffsetMap["ExportedSymbolGroupArray"]);

		for (auto& group : gameSymbolGroups)
		{
			for (auto& member : group->m_Members)
			{
				// Dump functions and variables only - everything else is handled by GGRTTI
				if (member.m_Type != ExportedSymbolMember::MEMBER_TYPE_FUNCTION && member.m_Type != ExportedSymbolMember::MEMBER_TYPE_VARIABLE)
					continue;

				for (auto& info : member.m_Infos)
				{
					char fullDecl[1024] = {};
					bool parsedFirst = false;

					for (auto& signature : info.m_Signatures)
					{
						if (!parsedFirst)
						{
							// Function name plus return type
							sprintf_s(fullDecl, "%s%s %s(", signature.m_BaseType.c_str(), signature.m_Modifiers.c_str(), info.m_Name);
							parsedFirst = true;
						}
						else
						{
							// Parameters
							strcat_s(fullDecl, signature.m_BaseType.c_str());
							strcat_s(fullDecl, signature.m_Modifiers.c_str());
							strcat_s(fullDecl, ", ");
						}
					}

					if (parsedFirst)
						strcat_s(fullDecl, ");");

					fprintf(F, "set_name(0x%llX, \"%s\");// %s\n", reinterpret_cast<uintptr_t>(info.m_Address) - g_ModuleBase + 0x140000000, member.m_SymbolName, fullDecl);

					// Skip multiple localization entries for now
					break;
				}
			}
		}
	}
}