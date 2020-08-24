#include <regex>
#include "common.h"
#include "MSRTTI.h"
#include "PCore.h"
#include "ExportedSymbol.h"
#include "RTTIIDAExporter.h"

extern std::unordered_set<const RTTI *> AllRegisteredTypeInfo;

namespace RTTIIDAExporter
{
	void ExportAll(const char *Directory)
	{
		char outputFilePath[MAX_PATH];
		sprintf_s(outputFilePath, "%s\\HZD_IDA_RTTI.idc", Directory);

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
				uintptr_t vfunc = *(uintptr_t *)vfuncPointer;
				uintptr_t vfuncDBAddress = vfunc - g_ModuleBase + 0x140000000;

				// Make sure it's not a pure virtual function
				if (*(uint8_t *)vfunc == 0xFF && *(uint8_t *)(vfunc + 1) == 0x25)
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

				uintptr_t address = (uintptr_t)Pointer - g_ModuleBase + 0x140000000;
				fprintf(F, Format, address, Name, address);
			};

			printDBOffset(type, "set_name(0x%llX, \"GGRTTI_%s_%llX\");\n", symbolName.c_str());

			if (type->m_InfoType == RTTI::INFO_TYPE_CLASS)
			{
				for (auto& event : type->ClassEventSubscriptions())
				{
					auto formatStr = symbolName + "::" + event.m_Type->GetSymbolName();
					printDBOffset(event.m_Callback, "set_name(0x%llX, \"%sCallback_%llX\");\n", formatStr.c_str());
				}

				printDBOffset(type->Class.m_Constructor, "set_name(0x%llX, \"%s::RTTIConstructor_%llX\");\n", symbolName.c_str());
				printDBOffset(type->Class.m_Destructor, "set_name(0x%llX, \"%s::RTTIDestructor_%llX\");\n", symbolName.c_str());
				printDBOffset(type->Class.m_Deserialize, "set_name(0x%llX, \"%s::RTTIDeserializeText_%llX\");\n", symbolName.c_str());
				printDBOffset(type->Class.m_Serialize, "set_name(0x%llX, \"%s::RTTISerializeText_%llX\");\n", symbolName.c_str());
			}
		}

		/*
		// Enum and class members
		for (auto typeInfo : AllRegisteredTypeInfo)
		{
			if (typeInfo->m_InfoType == RTTI::INFO_TYPE_ENUM || typeInfo->m_InfoType == RTTI::INFO_TYPE_ENUM_2)
			{
				fprintf(F, "// Binary type = 0x%llX\n", typeInfo->GetCoreBinaryTypeId_UNSAFE());
				fprintf(F, "enum %s\n{\n", typeInfo->GetSymbolName().c_str());

				for (auto& member : typeInfo->EnumMembers())
					fprintf(F, "\t%s = %d,\n", member.m_Name, member.m_Value);

				fprintf(F, "};\n\n");
			}
			else if (typeInfo->m_InfoType == RTTI::INFO_TYPE_CLASS)
			{
				char inheritanceDecl[1024] = {};

				if (typeInfo->m_ClassInheritanceCount)
				{
					strcat_s(inheritanceDecl, " : ");

					for (int i = 0; i < typeInfo->m_ClassInheritanceCount; i++)
					{
						strcat_s(inheritanceDecl, typeInfo->Class.m_InheritanceData[i].m_Type->GetSymbolName().c_str());

						if (i < (typeInfo->m_ClassInheritanceCount - 1))
							strcat_s(inheritanceDecl, ", ");
					}
				}

				fprintf(F, "// Binary type = 0x%llX\n", typeInfo->GetCoreBinaryTypeId_UNSAFE());
				fprintf(F, "// sizeof() = 0x%X\n", typeInfo->Class.m_Size);

				for (auto& event : typeInfo->ClassEventSubscriptions())
					fprintf(F, "// Event callback: %s = 0x%llX\n", event.m_Type->GetSymbolName().c_str(), (uintptr_t)event.m_Callback - g_ModuleBase + 0x140000000);

				fprintf(F, "class %s%s\n{\npublic:\n", typeInfo->GetSymbolName().c_str(), inheritanceDecl);

				for (auto& member : typeInfo->ClassMembers())
				{
					if (member.m_Type)
					{
						// Regular data member
						fprintf(F, "\t%s %s;// 0x%X\n", member.m_Type->GetSymbolName().c_str(), member.m_Name, member.m_Offset);
					}
					else
					{
						// Internal marker
						fprintf(F, "\t// Category: %s\n", member.m_Name);
					}
				}

				fprintf(F, "};\n\n");
			}
		*/
	}

	void ExportGameSymbolRTTI(FILE * F)
	{
		auto& gameSymbolGroups = *(Array<ExportedSymbolGroup *> *)(g_ModuleBase + 0x2A1F870);

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

					fprintf(F, "set_name(0x%llX, \"%s\");// %s\n", (uintptr_t)info.m_Address - g_ModuleBase + 0x140000000, member.m_SymbolName, fullDecl);

					// Skip multiple localization entries for now
					break;
				}
			}
		}
	}
}