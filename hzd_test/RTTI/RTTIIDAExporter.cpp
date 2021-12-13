#include <windows.h>
#include <regex>

#include "MSRTTI.h"
#include "RTTIIDAExporter.h"

using namespace HRZ;

constexpr uintptr_t IDABaseAddressExe = 0x140000000;
constexpr uintptr_t IDABaseAddressFullgame = 0x180000000;

RTTIIDAExporter::RTTIIDAExporter(const std::unordered_set<const RTTI *>& Types, const std::string_view GameTypePrefix) : m_Types(Types), m_GameTypePrefix(GameTypePrefix)
{
	m_ModuleBase = Offsets::GetModule().first;
}

void RTTIIDAExporter::ExportRTTITypes(const std::string_view Directory)
{
	CreateDirectoryA(Directory.data(), nullptr);

	auto outputPath = std::format("{0:}\\IDA_{1:}_Typeinfo.idc", Directory, m_GameTypePrefix);

	if (fopen_s(&m_FileHandle, outputPath.c_str(), "w") == 0)
	{
		Print("#include <idc.idc>\n\n");
		Print("static main()\n{{\n");

		ExportMSRTTI();
		ExportGGRTTIStructures();
		ExportGGRTTI();
		ExportGameSymbolRTTI();

		Print("}}\n");
		fclose(m_FileHandle);
	}
}

void RTTIIDAExporter::ExportFullgameTypes(const std::string_view Directory)
{
	CreateDirectoryA(Directory.data(), nullptr);

	auto outputPath = std::format("{0:}\\IDA_{1:}_Fullgame_Typeinfo.idc", Directory, m_GameTypePrefix);

	if (fopen_s(&m_FileHandle, outputPath.c_str(), "w") == 0)
	{
		Print("#include <idc.idc>\n\n");
		Print("static main()\n{{\n");

		ExportFullgameScriptSymbols();

		Print("}}\n");
		fclose(m_FileHandle);
	}
}

void RTTIIDAExporter::ExportMSRTTI()
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
			uintptr_t vfuncDBAddress = vfunc - m_ModuleBase + IDABaseAddressExe;

			// Make sure it's not a pure virtual function
			if (*reinterpret_cast<uint8_t *>(vfunc + 0) == 0xFF && *reinterpret_cast<uint8_t *>(vfunc + 1) == 0x25)
				Print("set_name({0:#X}, \"_purecall\");\n", vfuncDBAddress);
			else
				Print("set_name({0:#X}, \"{1:}::VFunc{2:02d}_{3:X}\");\n", vfuncDBAddress, className, i, vfuncDBAddress);
		}
	}
}

void RTTIIDAExporter::ExportGGRTTIStructures()
{
	// One day C++ will have reflection...
	Print("auto id = 0; auto mid = 0;\n");

#define REFL_STRUCT(Structure, MemberDecls) \
	{ \
		using StructType = Structure; \
		Print("del_struc(get_struc_id(\"{0:}\"));\n", #Structure); \
		Print("id = add_struc(-1, \"{0:}\", 0);\n", #Structure); \
		MemberDecls \
		Print("if (get_struc_size(id) < {0:#X}) add_struc_member(id, \"__padding\", {0:#X} - 1, 0, -1, 1);\n", sizeof(StructType), sizeof(StructType)); \
		Print("set_struc_align(id, {0:});\n", static_cast<int>(std::log2(alignof(StructType)))); \
	}
#define REFL_MEMBER(Member) WriteReflectedMemberT<decltype(StructType::Member)>(#Member, offsetof(StructType, Member), nullptr)

	REFL_STRUCT(RTTI,
	{
		REFL_MEMBER(m_RuntimeTypeId1);
		REFL_MEMBER(m_RuntimeTypeId2);
		REFL_MEMBER(m_InfoType);
		REFL_MEMBER(m_EnumUnderlyingTypeSize);
		REFL_MEMBER(m_EnumMemberCount);
	})

	REFL_STRUCT(RTTIPrimitive,
	{
		WriteReflectedMemberT<RTTI>("base", 0, "RTTI");
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

	REFL_STRUCT(RTTIContainer,
	{
		WriteReflectedMemberT<RTTI>("base", 0, "RTTI");
		REFL_MEMBER(m_Type);
		REFL_MEMBER(m_Data);
	});

	REFL_STRUCT(RTTIContainer::ContainerData,
	{
		REFL_MEMBER(m_Name);
	});

	REFL_STRUCT(RTTIEnum,
	{
		WriteReflectedMemberT<RTTI>("base", 0, "RTTI");
		REFL_MEMBER(m_Name);
		REFL_MEMBER(m_Values);
	});

	REFL_STRUCT(RTTIEnum::EnumEntry,
	{
		REFL_MEMBER(m_Value);
		REFL_MEMBER(m_Name);
	});

	REFL_STRUCT(RTTIClass,
	{
		WriteReflectedMemberT<RTTI>("base", 0, "RTTI");
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

	REFL_STRUCT(RTTIClass::InheritanceEntry,
	{
		REFL_MEMBER(m_Type);
		REFL_MEMBER(m_Offset);
	});

	REFL_STRUCT(RTTIClass::MemberEntry,
	{
		REFL_MEMBER(m_Type);
		REFL_MEMBER(m_Offset);
		REFL_MEMBER(m_Flags);
		REFL_MEMBER(m_Name);
		REFL_MEMBER(m_PropertyGetter);
		REFL_MEMBER(m_PropertySetter);
	});

	REFL_STRUCT(RTTIClass::LuaFunctionEntry,
	{
		REFL_MEMBER(m_ReturnValueType);
		REFL_MEMBER(m_Name);
		REFL_MEMBER(m_ArgumentString);
		REFL_MEMBER(m_Function);
	});

	REFL_STRUCT(RTTIClass::MessageHandlerEntry,
	{
		REFL_MEMBER(m_Type);
		REFL_MEMBER(m_Callback);
	});

	REFL_STRUCT(RTTIClass::InheritedMessageEntry,
	{
		REFL_MEMBER(m_Unknown);
		REFL_MEMBER(m_Type);
		REFL_MEMBER(m_ClassType);
	});

	REFL_STRUCT(RTTIPOD,
	{
		WriteReflectedMemberT<RTTI>("base", 0, "RTTI");
		REFL_MEMBER(m_Size);
	});

#undef REFL_MEMBER
#undef REFL_STRUCT
}

void RTTIIDAExporter::ExportGGRTTI()
{
	std::unordered_set<const RTTI *> visitedRTTITypes;

	// RTTI metadata
	for (auto& type : m_Types)
	{
		auto pprint = [&]<typename... TArgs>(const void *Pointer, std::string_view Format, TArgs&&... Args)
		{
			if (!Pointer)
				return;

			auto address = reinterpret_cast<uintptr_t>(Pointer) - m_ModuleBase + IDABaseAddressExe;
			Print(Format, address, std::forward<TArgs>(Args)...);
			Print("\n");
		};

		auto exportTable = [&]<typename T>(const std::span<T>& Span, std::string_view Symbol, std::string_view Name)
		{
			if (Span.empty())
				return;

			auto typeIdName = std::regex_replace(typeid(T).name(), std::regex("class |HRZ::"), "");
			pprint(Span.data(), "del_items({0:#X}, DELIT_SIMPLE, {1:});", Span.size_bytes());
			pprint(Span.data(), "create_struct({0:#X}, -1, \"{1:}\");", typeIdName);
			pprint(Span.data(), "make_array({0:#X}, {1:});", Span.size());
			pprint(Span.data(), "set_name({0:#X}, \"{1:}::{2:}_{0:X}\");", Symbol, Name);
		};

		std::function<void(const RTTI *)> visitType = [&](const RTTI *Type)
		{
			if (visitedRTTITypes.contains(type))
				return;

			visitedRTTITypes.emplace(Type);

			auto rttiTypeName = type->GetRTTITypeName();
			auto symbolName = type->GetSymbolName();

			pprint(type, "del_items({0:#X}, DELIT_SIMPLE, get_struc_size(get_struc_id(\"{1:}\")));", rttiTypeName);
			pprint(type, "create_struct({0:#X}, -1, \"{1:}\");", rttiTypeName);
			pprint(type, "set_name({0:#X}, \"RTTI_{1:}_{0:X}\", SN_CHECK);// {2:#X}", symbolName, type->GetCoreBinaryTypeId());

			if (auto asEnum = type->AsEnum(); asEnum)
			{
				exportTable(asEnum->EnumMembers(), symbolName, "Values");// m_Values
			}
			else if (auto asClass = type->AsClass(); asClass)
			{
				for (auto& event : asClass->ClassMessageHandlers())
				{
					visitType(event.m_Type);

					pprint(event.m_Callback, "set_name({0:#X}, \"{1:}::{2:}Callback_{0:X}\");", symbolName, event.m_Type->GetSymbolName());
				}

				for (auto& member : asClass->ClassMembers())
				{
					if (!member.m_Type)
						continue;

					visitType(member.m_Type);

					pprint(member.m_PropertyGetter, "set_name({0:#X}, \"{1:}::{2:}Getter_{0:X}\");", symbolName, member.m_Name);
					pprint(member.m_PropertySetter, "set_name({0:#X}, \"{1:}::{2:}Setter_{0:X}\");", symbolName, member.m_Name);
				}

				for (auto& luaFunc : asClass->ClassLuaFunctions())
				{
					pprint(luaFunc.m_Function, "set_name({0:#X}, \"{1:}::{2:}Lua_{0:X}\");", symbolName, luaFunc.m_Name);
				}

				pprint(asClass->m_Constructor, "set_name({0:#X}, \"{1:}::RTTIConstructor_{0:X}\");", symbolName);
				pprint(asClass->m_Destructor, "set_name({0:#X}, \"{1:}::RTTIDestructor_{0:X}\");", symbolName);
				pprint(asClass->m_DeserializeString, "set_name({0:#X}, \"{1:}::RTTIDeserializeString_{0:X}\");", symbolName);
				pprint(asClass->m_SerializeString, "set_name({0:#X}, \"{1:}::RTTISerializeText_{0:X}\");", symbolName);
				exportTable(asClass->ClassInheritance(), symbolName, "InheritanceTable");// m_InheritanceTable
				exportTable(asClass->ClassMembers(), symbolName, "MemberTable");// m_MemberTable
				exportTable(asClass->ClassLuaFunctions(), symbolName, "LuaFunctionTable");// m_LuaFunctionTable
				exportTable(asClass->ClassMessageHandlers(), symbolName, "MessageHandlerTable");// m_MessageHandlerTable
				exportTable(asClass->ClassInheritedMessages(), symbolName, "InheritedMessageTable");// m_InheritedMessageTable
				pprint(asClass->m_GetExportedSymbols, "set_name({0:#X}, \"{1:}::GetExportedSymbols_{0:X}\");", symbolName);

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

	Print("/*\n");

	// Enum and class members
	for (auto type : m_Types)
	{
		if (auto asEnum = type->AsEnum(); asEnum)
		{
			Print("// Binary type = 0x{0:X}\n", asEnum->GetCoreBinaryTypeId());
			Print("// sizeof() = 0x{0:X}\n", asEnum->m_EnumUnderlyingTypeSize);
			Print("enum {0:}\n{{\n", asEnum->GetSymbolName());

			for (auto& member : asEnum->EnumMembers())
				Print("\t{0:} = {1:},\n", member.m_Name, member.m_Value);

			Print("}};\n\n");
		}
		else if (auto asClass = type->AsClass(); asClass)
		{
			Print("// Binary type = 0x{0:X}\n", asClass->GetCoreBinaryTypeId());
			Print("// sizeof() = 0x{0:X} | alignof() = 0x{1:X}\n", asClass->m_Size, asClass->m_Alignment);

			auto inheritanceInfo = asClass->ClassInheritance();
			char inheritanceDecl[1024] = {};

			if (!inheritanceInfo.empty())
			{
				strcat_s(inheritanceDecl, " : ");

				for (auto& inherited : inheritanceInfo)
				{
					strcat_s(inheritanceDecl, inherited.m_Type->GetSymbolName().c_str());
					strcat_s(inheritanceDecl, ", ");

					Print("// Base: {0:} = 0x{1:X}\n", inherited.m_Type->GetSymbolName(), inherited.m_Offset);
				}

				*strrchr(inheritanceDecl, ',') = '\0';
			}

			for (auto& event : asClass->ClassMessageHandlers())
				Print("// Event callback: {0:} = 0x{1:X}\n", event.m_Type->GetSymbolName(), reinterpret_cast<uintptr_t>(event.m_Callback) - m_ModuleBase + IDABaseAddressExe);

			Print("class {0:}{1:}\n{{\npublic:\n", asClass->GetSymbolName(), inheritanceDecl);

			// Dump all class members
			auto members = asClass->GetCategorizedClassMembers();

			for (auto& [member, category, _] : members)
			{
				// Regular data member
				Print("\t{0:} {1:};// 0x{2:X}", member->m_Type->GetSymbolName(), member->m_Name, member->m_Offset);

				if (member->m_PropertyGetter || member->m_PropertySetter)
					Print(" (Property)");

				if (member->IsSaveStateOnly())
					Print(" (SAVE_STATE_ONLY)");

				Print("\n");
			}

			Print("}};\n\n");
		}
	}

	for (auto& type : m_Types)
	{
		if (auto asClass = type->AsClass(); asClass)
		{
			Print("const RTTI *RTTI_{0:} = Offsets::Resolve<const RTTI *>(0x{1:X});\n", asClass->GetSymbolName(), reinterpret_cast<uintptr_t>(asClass) - m_ModuleBase);
		}
	}

	Print("*/\n");
}

void RTTIIDAExporter::ExportGameSymbolRTTI()
{
	auto& gameSymbolGroups = *Offsets::ResolveID<"ExportedSymbolGroupArray", Array<ExportedSymbolGroup *> *>();

	for (auto& group : gameSymbolGroups)
	{
		for (auto& member : group->m_Members)
		{
			// Dump functions and variables only - everything else is handled by RTTI
			if (member.m_Type != ExportedSymbolMember::MEMBER_TYPE_FUNCTION && member.m_Type != ExportedSymbolMember::MEMBER_TYPE_VARIABLE)
				continue;

			for (auto& info : member.m_Infos)
			{
				auto fullDeclaration = BuildGameSymbolFunctionDecl(info, false);
				Print("set_name({0:#X}, \"{1:}\");// {2:};\n", reinterpret_cast<uintptr_t>(info.m_Address) - m_ModuleBase + IDABaseAddressExe, member.m_SymbolName, fullDeclaration);

				// Skip multiple localization entries for now
				break;
			}
		}
	}
}

void RTTIIDAExporter::ExportFullgameScriptSymbols()
{
	auto baseAddress = reinterpret_cast<uintptr_t>(GetModuleHandleA("fullgame.dll"));

	if (!baseAddress)
		return;

	// Build symbol to address mapping
	auto& gameSymbolGroups = *Offsets::ResolveID<"ExportedSymbolGroupArray", Array<ExportedSymbolGroup *> *>();
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
		auto asRTTI = reinterpret_cast<const RTTI *>(symbolPointer);

		if (auto itr = symbolAddressMap.find(symbolPointer); itr != symbolAddressMap.end())
		{
			auto symNamespace = itr->second->m_SymbolNamespace ? itr->second->m_SymbolNamespace : "";

			Print("set_name({0:#X}, \"{1:}::{2:}\", SN_FORCE);\n", i - baseAddress + IDABaseAddressFullgame, symNamespace, itr->second->m_Infos[0].m_Name);
			Print("apply_type({0:#X}, \"{1:}\");\n", i - baseAddress + IDABaseAddressFullgame, BuildGameSymbolFunctionDecl(itr->second->m_Infos[0], true));
		}

		if (m_Types.contains(asRTTI))
		{
			Print("set_name({0:#X}, \"RTTI_{1:}\", SN_FORCE);\n", i - baseAddress + IDABaseAddressFullgame, asRTTI->GetSymbolName());
		}
	}
}

std::string RTTIIDAExporter::BuildReturnType(std::string_view BaseType, std::string_view Modifiers, bool IDAFormat)
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

std::string RTTIIDAExporter::BuildArgType(size_t Index, std::string BaseType, std::string_view Modifiers, bool IDAFormat)
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

std::string RTTIIDAExporter::BuildGameSymbolFunctionDecl(const ExportedSymbolMember::LanguageInfo& Info, bool IDAFormat)
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