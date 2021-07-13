#include <regex>
#include <format>

#include "HRZ/Core/GGRTTI.h"
#include "HRZ/Core/ExportedSymbol.h"

#include "common.h"
#include "MSRTTI.h"
#include "RTTIIDAExporter.h"

extern std::unordered_set<const HRZ::GGRTTI *> AllRegisteredTypeInfo;

namespace RTTIIDAExporter
{
constexpr uintptr_t IDABaseAddressExe = 0x140000000;
constexpr uintptr_t IDABaseAddressFullgame = 0x180000000;

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
			ExportGGRTTIStructures(f);
			ExportGGRTTI(f);
			ExportGameSymbolRTTI(f);

			fprintf(f, "}\n");
			fclose(f);
		}
	}

	template<typename... TArgs>
	void Print(FILE *F, const std::string_view Format, const TArgs&&... Args)
	{
		fprintf(F, "%s\n", std::format(Format, Args...).data());
	}

	void ExportMSRTTI(FILE *F)
	{
		// Order by most complex inheritance to least complex
		auto allRTTI = MSRTTI::FindAll(nullptr, false);

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
				uintptr_t vfuncDBAddress = vfunc - g_ModuleBase + IDABaseAddressExe;

				// Make sure it's not a pure virtual function
				if (*reinterpret_cast<uint8_t *>(vfunc + 0) == 0xFF && *reinterpret_cast<uint8_t *>(vfunc + 1) == 0x25)
					fprintf(F, "set_name(0x%llX, \"_purecall\");\n", vfuncDBAddress);
				else
					fprintf(F, "set_name(0x%llX, \"%s::VFunc%02lld_%llX\");\n", vfuncDBAddress, className.c_str(), i, vfuncDBAddress);
			}
		}
	}

	template<typename T>
	void WriteMemberT(FILE *F, const char *Name, size_t Offset, const char *Type)
	{
		uint32_t typeFlags = 0;

		switch (sizeof(T))
		{
		case 1: typeFlags = 0x0; break;// FF_BYTE
		case 2: typeFlags = 0x10000000; break;// FF_WORD
		case 4: typeFlags = 0x20000000; break;// FF_DWORD
		case 8: typeFlags = 0x30000000; break;// FF_QWORD
		}

		if (Type)
			typeFlags = 0x60000000;// FF_STRUCT

		if constexpr (std::is_pointer_v<T>)
			typeFlags |= 0x5500400;// FF_DATA | FF_0OFF | FF_1OFF (data offset)

		if (!Type)
			fprintf(F, "mid = add_struc_member(id, \"%s\", 0x%llX, 0x%X, -1, %lld);\n", Name, Offset, typeFlags, sizeof(T));
		else
			fprintf(F, "mid = add_struc_member(id, \"%s\", 0x%llX, 0x%X, get_struc_id(\"%s\"), %lld);\n", Name, Offset, typeFlags, Type, sizeof(T));
	}

	void ExportGGRTTIStructures(FILE *F)
	{
		// One day C++ will have reflection...
		fprintf(F, "auto id = 0; auto mid = 0;\n");

#define REFL_STRUCT(Structure, MemberDecls) \
	{ \
		using StructType = Structure; \
		fprintf(F, "del_struc(get_struc_id(\"%s\"));\n", #Structure); \
		fprintf(F, "id = add_struc(-1, \"%s\", 0);\n", #Structure); \
		MemberDecls \
		fprintf(F, "if (get_struc_size(id) < 0x%llX) add_struc_member(id, \"__padding\", 0x%llX - 1, 0, -1, 1);\n", sizeof(StructType), sizeof(StructType)); \
		fprintf(F, "set_struc_align(id, %d);\n", static_cast<int>(std::log2(alignof(StructType)))); \
	}
#define REFL_MEMBER(Member) WriteMemberT<decltype(StructType::Member)>(F, #Member, offsetof(StructType, Member), nullptr)

		REFL_STRUCT(GGRTTI,
		{
			REFL_MEMBER(m_RuntimeTypeId1);
			REFL_MEMBER(m_RuntimeTypeId2);
			REFL_MEMBER(m_InfoType);
			REFL_MEMBER(m_EnumUnderlyingTypeSize);
			REFL_MEMBER(m_EnumMemberCount);
		})

		REFL_STRUCT(GGRTTIPrimitive,
		{
			WriteMemberT<GGRTTI>(F, "base", 0, "GGRTTI");
			REFL_MEMBER(m_Name);
			REFL_MEMBER(m_ParentType);
			REFL_MEMBER(m_DeserializeString);
			REFL_MEMBER(m_SerializeString);
			REFL_MEMBER(m_SwapValues);
			REFL_MEMBER(m_TestEqualityValues);
			REFL_MEMBER(m_Constructor);
			REFL_MEMBER(m_UnknownFunction1);
			REFL_MEMBER(m_SwapEndianness);
			REFL_MEMBER(m_AssignValues);
			REFL_MEMBER(m_GetSize);
			REFL_MEMBER(m_CompareByStrings);
			REFL_MEMBER(m_UnknownFunction2);
		});

		REFL_STRUCT(GGRTTIContainer,
		{
			WriteMemberT<GGRTTI>(F, "base", 0, "GGRTTI");
			REFL_MEMBER(m_Type);
			REFL_MEMBER(m_Data);
		});

		REFL_STRUCT(GGRTTIContainer::ContainerData,
		{
			REFL_MEMBER(m_Name);
		});

		REFL_STRUCT(GGRTTIEnum,
		{
			WriteMemberT<GGRTTI>(F, "base", 0, "GGRTTI");
			REFL_MEMBER(m_Name);
			REFL_MEMBER(m_Values);
		});

		REFL_STRUCT(GGRTTIEnum::EnumEntry,
		{
			REFL_MEMBER(m_Value);
			REFL_MEMBER(m_Name);
		});

		REFL_STRUCT(GGRTTIClass,
		{
			WriteMemberT<GGRTTI>(F, "base", 0, "GGRTTI");
			REFL_MEMBER(m_MessageHandlerCount);
			REFL_MEMBER(m_Size);
			REFL_MEMBER(m_Alignment);
			REFL_MEMBER(m_Flags);
			REFL_MEMBER(m_Constructor);
			REFL_MEMBER(m_Destructor);
			REFL_MEMBER(m_DeserializeString);
			REFL_MEMBER(m_SerializeString);
			REFL_MEMBER(m_Name);
			REFL_MEMBER(m_InheritanceTable);
			REFL_MEMBER(m_MemberTable);
			REFL_MEMBER(m_LuaFunctionTable);
			REFL_MEMBER(m_MessageHandlerTable);
			REFL_MEMBER(m_InheritedMessageTable);
			REFL_MEMBER(m_GetExportedSymbols);
		});

		REFL_STRUCT(GGRTTIClass::InheritanceEntry,
		{
			REFL_MEMBER(m_Type);
			REFL_MEMBER(m_Offset);
		});

		REFL_STRUCT(GGRTTIClass::MemberEntry,
		{
			REFL_MEMBER(m_Type);
			REFL_MEMBER(m_Offset);
			REFL_MEMBER(m_Flags);
			REFL_MEMBER(m_Name);
			REFL_MEMBER(m_PropertyGetter);
			REFL_MEMBER(m_PropertySetter);
		});

		REFL_STRUCT(GGRTTIClass::LuaFunctionEntry,
		{
			REFL_MEMBER(m_ReturnValueType);
			REFL_MEMBER(m_Name);
			REFL_MEMBER(m_ArgumentString);
			REFL_MEMBER(m_Function);
		});

		REFL_STRUCT(GGRTTIClass::MessageHandlerEntry,
		{
			REFL_MEMBER(m_Type);
			REFL_MEMBER(m_Callback);
		});

		REFL_STRUCT(GGRTTIClass::InheritedMessageEntry,
		{
			REFL_MEMBER(m_Unknown);
			REFL_MEMBER(m_Type);
			REFL_MEMBER(m_ClassType);
		});

		REFL_STRUCT(GGRTTIPOD,
		{
			WriteMemberT<GGRTTI>(F, "base", 0, "GGRTTI");
			REFL_MEMBER(m_Size);
		});

#undef REFL_MEMBER
#undef REFL_STRUCT
	}

	void ExportGGRTTI(FILE *F)
	{
		std::unordered_set<const GGRTTI *> visitedRTTITypes;

		// RTTI metadata
		for (auto& type : AllRegisteredTypeInfo)
		{
			auto pprint = [&]<typename... TArgs>(const void *Pointer, const std::string_view Format, const TArgs&... Args)
			{
				if (!Pointer)
					return;

				auto address = reinterpret_cast<uintptr_t>(Pointer) - g_ModuleBase + IDABaseAddressExe;
				fprintf(F, "%s\n", std::format(Format, address, Args...).data());
			};

			auto exportTable = [&]<typename T>(const std::span<T>& Span, const std::string_view Symbol, const std::string_view Name)
			{
				if (Span.empty())
					return;

				auto typeIdName = std::regex_replace(typeid(T).name(), std::regex("class "), "");
				pprint(Span.data(), "del_items(0x{0:X}, DELIT_SIMPLE, {1:});", Span.size_bytes());
				pprint(Span.data(), "create_struct(0x{0:X}, -1, \"{1:}\");", typeIdName);
				pprint(Span.data(), "make_array(0x{0:X}, {1:});", Span.size());
				pprint(Span.data(), "set_name(0x{0:X}, \"{1:}::{2:}_{0:X}\");", Symbol, Name);
			};

			std::function<void(const GGRTTI *)> visitType = [&](const GGRTTI *Type)
			{
				if (visitedRTTITypes.contains(type))
					return;

				visitedRTTITypes.emplace(Type);

				auto rttiTypeName = type->GetRTTITypeName();
				auto symbolName = type->GetSymbolName();

				pprint(type, "del_items(0x{0:X}, DELIT_SIMPLE, get_struc_size(get_struc_id(\"{1:}\")));", rttiTypeName);
				pprint(type, "create_struct(0x{0:X}, -1, \"{1:}\");", rttiTypeName);
				pprint(type, "set_name(0x{0:X}, \"GGRTTI_{1:}_{0:X}\", SN_CHECK);// 0x{2:X}", symbolName, type->GetCoreBinaryTypeId());

				if (auto asEnum = type->AsEnum(); asEnum)
				{
					exportTable(asEnum->EnumMembers(), symbolName, "Values");// m_Values
				}
				else if (auto asClass = type->AsClass(); asClass)
				{
					for (auto& event : asClass->ClassMessageHandlers())
					{
						visitType(event.m_Type);

						pprint(event.m_Callback, "set_name(0x{0:X}, \"{1:}::{2:}Callback_{0:X}\");", symbolName, event.m_Type->GetSymbolName());
					}

					for (auto& member : asClass->ClassMembers())
					{
						if (!member.m_Type)
							continue;

						visitType(member.m_Type);

						pprint(member.m_PropertyGetter, "set_name(0x{0:X}, \"{1:}::{2:}Getter_{0:X}\");", symbolName, member.m_Name);
						pprint(member.m_PropertySetter, "set_name(0x{0:X}, \"{1:}::{2:}Setter_{0:X}\");", symbolName, member.m_Name);
					}

					for (auto& luaFunc : asClass->ClassLuaFunctions())
					{
						pprint(luaFunc.m_Function, "set_name(0x{0:X}, \"{1:}::{2:}Lua_{0:X}\");", symbolName, luaFunc.m_Name);
					}

					pprint(asClass->m_Constructor, "set_name(0x{0:X}, \"{1:}::RTTIConstructor_{0:X}\");", symbolName);
					pprint(asClass->m_Destructor, "set_name(0x{0:X}, \"{1:}::RTTIDestructor_{0:X}\");", symbolName);
					pprint(asClass->m_DeserializeString, "set_name(0x{0:X}, \"{1:}::RTTIDeserializeString_{0:X}\");", symbolName);
					pprint(asClass->m_SerializeString, "set_name(0x{0:X}, \"{1:}::RTTISerializeText_{0:X}\");", symbolName);
					exportTable(asClass->ClassInheritance(), symbolName, "InheritanceTable");// m_InheritanceTable
					exportTable(asClass->ClassMembers(), symbolName, "MemberTable");// m_MemberTable
					exportTable(asClass->ClassLuaFunctions(), symbolName, "LuaFunctionTable");// m_LuaFunctionTable
					exportTable(asClass->ClassMessageHandlers(), symbolName, "MessageHandlerTable");// m_MessageHandlerTable
					exportTable(asClass->ClassInheritedMessages(), symbolName, "InheritedMessageTable");// m_InheritedMessageTable
					pprint(asClass->m_GetExportedSymbols, "set_name(0x{0:X}, \"{1:}::GetExportedSymbols_{0:X}\");", symbolName);

					if (asClass->m_GetExportedSymbols)
						visitType(asClass->m_GetExportedSymbols());

				}
				else if (auto containedType = type->GetContainedType(); containedType)
				{
					visitType(containedType);
				}
			};

			visitType(type);
		}

		fprintf(F, "/*\n");

		// Enum and class members
		for (auto type : AllRegisteredTypeInfo)
		{
			if (auto asEnum = type->AsEnum(); asEnum)
			{
				fprintf(F, "// Binary type = 0x%llX\n", asEnum->GetCoreBinaryTypeId());
				fprintf(F, "// sizeof() = 0x%X\n", asEnum->m_EnumUnderlyingTypeSize);
				fprintf(F, "enum %s\n{\n", asEnum->GetSymbolName().c_str());

				for (auto& member : asEnum->EnumMembers())
					fprintf(F, "\t%s = %d,\n", member.m_Name, member.m_Value);

				fprintf(F, "};\n\n");
			}
			else if (auto asClass = type->AsClass(); asClass)
			{
				auto inheritanceInfo = asClass->ClassInheritance();
				char inheritanceDecl[1024] = {};

				fprintf(F, "// Binary type = 0x%llX\n", asClass->GetCoreBinaryTypeId());
				fprintf(F, "// sizeof() = 0x%X | alignof() = 0x%X\n", asClass->m_Size, asClass->m_Alignment);

				if (!inheritanceInfo.empty())
				{
					strcat_s(inheritanceDecl, " : ");

					for (auto& inherited : inheritanceInfo)
					{
						strcat_s(inheritanceDecl, inherited.m_Type->GetSymbolName().c_str());
						strcat_s(inheritanceDecl, ", ");

						fprintf(F, "// Base: %s = 0x%X\n", inherited.m_Type->GetSymbolName().c_str(), inherited.m_Offset);
					}

					*strrchr(inheritanceDecl, ',') = '\0';
				}

				for (auto& event : asClass->ClassMessageHandlers())
					fprintf(F, "// Event callback: %s = 0x%llX\n", event.m_Type->GetSymbolName().c_str(), reinterpret_cast<uintptr_t>(event.m_Callback) - g_ModuleBase + IDABaseAddressExe);

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

		for (auto& type : AllRegisteredTypeInfo)
		{
			if (auto asClass = type->AsClass(); asClass)
			{
				fprintf(F, "const GGRTTI *RTTI_%s = ResolveOffset<const GGRTTI *>(0x%llX);\n", asClass->GetSymbolName().c_str(), reinterpret_cast<uintptr_t>(asClass) - g_ModuleBase);
			}
		}

		fprintf(F, "*/\n");
	}

	std::string BuildReturnType(std::string_view BaseType, std::string_view Modifiers, bool IDAFormat)
	{
		if (IDAFormat)
		{
			//
			// __int64 (__usercall *)@<rax>(struct Entity *@<rcx>, struct Entity const *@<rdx>, struct LevelBasedXpReward const *@<r8>, struct Entity *@<r9>, struct Entity *, bool)
			// float (__usercall *)@<xmm0>(struct Entity *@<rcx>, struct Entity const *@<rdx>, struct LevelBasedXpReward const *@<r8>, struct Entity *@<r9>, struct Entity *, bool)
			// void (__usercall *)(struct Entity *@<rcx>, struct Entity const *@<rdx>, struct LevelBasedXpReward const *@<r8>, struct Entity *@<r9>, struct Entity *, bool)
			//
			std::string_view returnLoc = "@<rax>";

			if (!Modifiers.empty())
				BaseType = "__int64";

			if (BaseType == "void")
				returnLoc = "";
			else if (BaseType == "float" || BaseType == "double")
				returnLoc = "@<xmm0>";
			else if (BaseType != "__int64" && BaseType != "bool" && BaseType != "int")
				BaseType = "__int64";

			return std::format("{0:} (__usercall *){1:}", BaseType, returnLoc);
		}

		return std::format("{0:}{1:} ", BaseType, Modifiers);
	}

	std::string BuildArgType(size_t Index, std::string BaseType, std::string_view Modifiers, bool IDAFormat)
	{
		if (IDAFormat)
		{
			if (!Modifiers.empty())
				BaseType = "__int64";

			if (BaseType == "float" || BaseType == "double")
			{
				switch (Index)
				{
				case 0: return BaseType + "@<xmm0>";
				case 1: return BaseType + "@<xmm1>";
				case 2: return BaseType + "@<xmm2>";
				case 3: return BaseType + "@<xmm3>";
				}

				return BaseType;
			}

			switch (Index)
			{
			case 0: return "__int64@<rcx>";
			case 1: return "__int64@<rdx>";
			case 2: return "__int64@<r8>";
			case 3: return "__int64@<r9>";
			}

			// It seems like I have to manually force the stack arguments too. Options > Compiler > Compiler > Visual C++.
			// https://hex-rays.com/products/ida/support/idadoc/1492.shtml ("Scattered Argument Locations")
			return std::format("__int64@<0:^{0:}.8>", (Index - 4) * sizeof(void *));
		}

		return std::format("{0:}{1:}", BaseType, Modifiers);
	}

	std::string BuildGameSymbolFunctionDecl(const ExportedSymbolMember::LanguageInfo& Info, bool IDAFormat)
	{
		std::string decl;

		for (size_t i = 0; i < Info.m_Signature.size(); i++)
		{
			auto& signature = Info.m_Signature[i];

			if (i == 0)
			{
				decl = BuildReturnType(signature.m_BaseType.c_str(), signature.m_Modifiers.c_str(), IDAFormat);

				if (!IDAFormat)
					decl += Info.m_Name;

				decl += "(";
			}
			else
			{
				decl += BuildArgType(i - 1, signature.m_BaseType.c_str(), signature.m_Modifiers.c_str(), IDAFormat);
				decl += ", ";
			}
		}

		decl += ")";
		return std::regex_replace(decl, std::regex("\\, \\)"), ")");
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
					auto fullDeclaration = BuildGameSymbolFunctionDecl(info, false);
					fprintf(F, "set_name(0x%llX, \"%s\");// %s;\n", reinterpret_cast<uintptr_t>(info.m_Address) - g_ModuleBase + 0x140000000, member.m_SymbolName, fullDeclaration.c_str());

					// Skip multiple localization entries for now
					break;
				}
			}
		}
	}

	void ExportFullgameScriptSymbols(FILE *F)
	{
		auto baseAddress = reinterpret_cast<uintptr_t>(GetModuleHandleA("fullgame.dll"));

		if (!baseAddress)
			return;

		// Build symbol to address mapping
		auto& gameSymbolGroups = *reinterpret_cast<Array<ExportedSymbolGroup *> *>(g_OffsetMap["ExportedSymbolGroupArray"]);
		std::unordered_map<uintptr_t, const ExportedSymbolMember *> symbolAddressMap;

		for (auto& group : gameSymbolGroups)
		{
			for (auto& member : group->m_Members)
			{
				if (member.m_Type != ExportedSymbolMember::MEMBER_TYPE_FUNCTION && member.m_Type != ExportedSymbolMember::MEMBER_TYPE_VARIABLE)
					continue;

				for (auto& info : member.m_Infos)
				{
					if (!info.m_Address)
						continue;

					symbolAddressMap.try_emplace(reinterpret_cast<uintptr_t>(info.m_Address), &member);
				}
			}
		}

		for (uintptr_t i = (baseAddress + 0xA465FE0); i < (baseAddress + 0xA46A000); i++)
		{
			auto symbolPointer = *reinterpret_cast<uintptr_t *>(i);
			auto asRTTI = reinterpret_cast<const GGRTTI *>(symbolPointer);

			if (auto itr = symbolAddressMap.find(symbolPointer); itr != symbolAddressMap.end())
			{
				fprintf(F, "set_name(0x%llX, \"%s::%s\", SN_FORCE);\n", i - baseAddress + IDABaseAddressFullgame, itr->second->m_SymbolNamespace, itr->second->m_Infos[0].m_Name);
				fprintf(F, "apply_type(0x%llX, \"%s\");\n", i - baseAddress + IDABaseAddressFullgame, BuildGameSymbolFunctionDecl(itr->second->m_Infos[0], true).c_str());
			}

			if (AllRegisteredTypeInfo.contains(asRTTI))
			{
				fprintf(F, "set_name(0x%llX, \"GGRTTI_%s\", SN_FORCE);\n", i - baseAddress + IDABaseAddressFullgame, asRTTI->GetSymbolName().c_str());
			}
		}
	}
}