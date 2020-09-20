#include "common.h"
#include "RTTICSharpExporter.h"

extern std::unordered_set<const RTTI *> AllRegisteredTypeInfo;

namespace RTTICSharpExporter
{
	void ExportAll(const char *Directory)
	{
		CreateDirectoryA(Directory, nullptr);

		// Build a list of all {classes|enums}, sorted by name
		std::vector<const RTTI *> sortedTypes;

		for (auto& type : AllRegisteredTypeInfo)
		{
			switch (type->m_InfoType)
			{
			case RTTI::INFO_TYPE_CLASS:
			case RTTI::INFO_TYPE_ENUM:
			case RTTI::INFO_TYPE_ENUM_2:
				sortedTypes.emplace_back(type);
				break;
			}
		}

		std::sort(sortedTypes.begin(), sortedTypes.end(), [](const RTTI *A, const RTTI *B)
		{
			return A->GetSymbolName() < B->GetSymbolName();
		});

		// Dump these types into their own separate files
		const char *separatedTypes[] =
		{
			"DataBufferResource",
			"GGUUID",
			"IndexArrayResource",
			"IVec2",
			"LocalizedSimpleSoundResource",
			"LocalizedTextResource",
			"MorphemeAnimationResource",
			"MusicResource",
			"PhysicsRagdollResource",
			"PhysicsShapeResource",
			"Pose",
			"ShaderResource",
			"Texture",
			"TextureList",
			"UITexture",
			"Vec2",
			"VertexArrayResource",
			"WaveResource",
		};

		sortedTypes.erase(std::remove_if(sortedTypes.begin(), sortedTypes.end(), [Directory, separatedTypes](const RTTI *Type)
		{
			for (auto name : separatedTypes)
			{
				if (Type->GetSymbolName() == name)
				{
					char outputFilePath[MAX_PATH];
					sprintf_s(outputFilePath, "%s\\Decima.HZD.%s.cs", Directory, name);

					if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
					{
						ExportFileHeader(f);
						ExportRTTIClass(f, Type);
						ExportFileFooter(f);
						fclose(f);
					}

					return true;
				}
			}

			return false;
		}), sortedTypes.end());

		// TODO: Split classes into separate files if they all reference a common base (i.e > 30 instances per)
		char outputFilePath[MAX_PATH];
		sprintf_s(outputFilePath, "%s\\Decima.HZD.AllStructs.cs", Directory);

		if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
		{
			ExportFileHeader(f);

			for (auto& type : sortedTypes)
			{
				if (type->m_InfoType == RTTI::INFO_TYPE_CLASS)
					ExportRTTIClass(f, type);
			}

			ExportFileFooter(f);
			fclose(f);
		}

		sprintf_s(outputFilePath, "%s\\Decima.HZD.AllEnums.cs", Directory);

		if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
		{
			ExportFileHeader(f);

			for (auto& type : sortedTypes)
			{
				if (type->m_InfoType == RTTI::INFO_TYPE_ENUM || type->m_InfoType == RTTI::INFO_TYPE_ENUM_2)
					ExportRTTIEnum(f, type);
			}

			ExportFileFooter(f);
			fclose(f);
		}
	}

	void ExportFileHeader(FILE *F)
	{
		const char *data =
			"#pragma warning disable CS0649 // warning CS0649: 'member' is never assigned to, and will always have its default value 'value'.\n"
			"#pragma warning disable CS0108 // warning CS0108: 'class' hides inherited member 'member'. Use the new keyword if hiding was intended.\n"
			"\n"
			"namespace Decima.HZD\n"
			"{\n"
			"    using int8 = System.SByte;\n"
			"    using uint8 = System.Byte;\n"
			"    using int16 = System.Int16;\n"
			"    using uint16 = System.UInt16;\n"
			"    using int32 = System.Int32;\n"
			"    using uint32 = System.UInt32;\n"
			"    using int64 = System.Int64;\n"
			"    using uint64 = System.UInt64;\n"
			"\n"
			"    using wchar = System.Int16;\n"
			"    using HalfFloat = System.UInt16;\n"
			"\n"
			"    using MaterialType = System.UInt16;\n"
			"    using AnimationTagID = System.UInt32;\n"
			"    using AnimationStateID = System.UInt32;\n"
			"    using AnimationEventID = System.UInt32;\n"
			"    using PhysicsCollisionFilterInfo = System.UInt32;\n"
			"\n";

		fputs(data, F);
	}

	void ExportFileFooter(FILE *F)
	{
		const char *data =
			"}\n";

		fputs(data, F);
	}

	void ExportRTTIEnum(FILE *F, const RTTI *Type)
	{
		// Attributes/decl
		fprintf(F, "[RTTI.Serializable(0x%llX)]\n", Type->GetCoreBinaryTypeId_UNSAFE());
		fprintf(F, "public enum %s : %s\n{\n", Type->GetSymbolName().c_str(), EnumTypeToString(Type));

		// Members
		size_t index = 0;

		for (auto& member : Type->EnumMembers())
		{
			std::string filteredName = member.m_Name;
			bool allUnderscores = true;

			// Strip parenthesis/commas/brackets
			for (char& c : filteredName)
			{
				if (!isalnum(c))
					c = '_';

				if (c != '_')
					allUnderscores = false;
			}

			// If it ends up as "____" with no letters, add an index to prevent collisions
			if (allUnderscores)
				filteredName += std::to_string(index);

			FilterMemberNameString(filteredName);

			fprintf(F, "\t%s = %d,\n", filteredName.c_str(), member.m_Value);
			index++;
		}

		fprintf(F, "}\n\n");
	}

	void ExportRTTIClass(FILE *F, const RTTI *Type)
	{
		// C# doesn't support multiple base classes, so pick one based on the order in RTTI data and treat the others as members (manual composition)
		char inheritanceDecl[1024] = {};
		auto inheritance = Type->ClassInheritance();
		bool postLoadCallback = IsPostLoadCallbackEnabled(Type);

		if (!inheritance.empty())
		{
			// TODO: Sort 'inheritance'?
			sprintf_s(inheritanceDecl, " : %s", inheritance[0].m_Type->GetSymbolName().c_str());
		}

		if (postLoadCallback)
		{
			if (strlen(inheritanceDecl) > 0)
				strcat_s(inheritanceDecl, ", RTTI.IExtraBinaryDataCallback");
			else
				strcat_s(inheritanceDecl, " : RTTI.IExtraBinaryDataCallback");
		}

		//
		// Possible declarations:
		//
		// public class AIAtmosphereBoxResource
		// public class AIAtmosphereBoxResource : Resource
		// public class AIAtmosphereBoxResource : Resource, RTTI.IExtraBinaryDataCallback
		// public class AIAtmosphereBoxResource : RTTI.IExtraBinaryDataCallback
		//
		char fullDecl[1024] = {};
		sprintf_s(fullDecl, "public class %s%s", Type->GetSymbolName().c_str(), inheritanceDecl);

		//
		// Possible attributes:
		//
		// [RTTI.Serializable(0xDC3D43D192F22E9B)]
		//
		fprintf(F, "[RTTI.Serializable(0x%llX)]\n", Type->GetCoreBinaryTypeId_UNSAFE());
		fprintf(F, "%s\n{\n", fullDecl);

		//
		// Possible members:
		//
		// [RTTI.Member(0, 0x0)] public WorldPosition CenterPosition;
		// [RTTI.Member(1, 0x74, "General")] public bool UsedForStealthGrass;
		// [RTTI.Member(1, 0x74, "General", true)] public bool UsedForStealthGrass;
		// [RTTI.Member(0, 0x20)] public GlobalRenderVariableValues @GlobalRenderVariableValues;
		// [RTTI.Member(0, 0x20, true)] public GlobalRenderVariableValues @GlobalRenderVariableValues;
		// [RTTI.BaseClass(0xC0)] public Shape2DExtrusion @Shape2DExtrusion;
		//
		int index = 0;

		// Insert fake members from base classes, skipping the first one
		for (size_t i = 1; i < inheritance.size(); i++)
		{
			auto name = inheritance[i].m_Type->GetSymbolName();

			if (IsBaseClassSuperfluous(inheritance[i].m_Type))
				continue;

			fprintf(F, "\t[RTTI.BaseClass(0x%X)] public %s @%s;\n", inheritance[i].m_Offset, name.c_str(), name.c_str());
			index++;
		}

		// Insert real members sorted by offset
		auto members = GetSortedClassMembers(Type);

		for (auto& [member, category] : members)
		{
			if (member->IsGroupMarker())
				continue;

			std::string typeName = member->m_Type->GetSymbolName();
			std::string memberName = member->m_Name;

			// Sometimes there are duplicate variable names in the same class. Add the category as a prefix.
			if (IsMemberNameDuplicated(Type, member))
				memberName = std::string(category) + "_" + memberName;

			// Sometimes the variable names and class names are identical. This isn't allowed.
			if (Type->GetSymbolName() == memberName)
				memberName = "_" + memberName;

			// Sometimes variable names start with a number. This isn't allowed either.
			FilterMemberNameString(memberName);

			char attributeDecl[1024] = {};

			if (strlen(category) > 0)
				sprintf_s(attributeDecl, "[RTTI.Member(%d, 0x%X, \"%s\"", index, member->m_Offset, category);
			else
				sprintf_s(attributeDecl, "[RTTI.Member(%d, 0x%X", index, member->m_Offset);

			if (member->IsSaveStateOnly())
				strcat_s(attributeDecl, ", true)]");
			else
				strcat_s(attributeDecl, ")]");

			fprintf(F, "\t%s public %s %s;\n", attributeDecl, typeName.c_str(), memberName.c_str());
			index++;
		}

		fprintf(F, "}\n\n");
	}

	const char *EnumTypeToString(const RTTI *Type)
	{
		if (Type->m_InfoType != RTTI::INFO_TYPE_ENUM && Type->m_InfoType != RTTI::INFO_TYPE_ENUM_2)
			__debugbreak();

		switch (Type->m_EnumUnderlyingTypeSize)
		{
		case 1:
			return "int8";

		case 2:
			return "int16";

		case 4:
			return "int32";

		case 8:
			return "int64";
		}

		__debugbreak();
		return "<INVALID>";
	}

	bool IsBaseClassSuperfluous(const RTTI *Type)
	{
		// Returns true if this type and all subtypes have no members listed in the binary format
		if (Type->m_InfoType != RTTI::INFO_TYPE_CLASS)
			__debugbreak();

		for (auto& base : Type->ClassInheritance())
		{
			if (!IsBaseClassSuperfluous(base.m_Type))
				return false;
		}

		for (auto& member : Type->ClassMembers())
		{
			if (!member.IsGroupMarker())
				return false;
		}

		return true;
	}

	void FilterMemberNameString(std::string& Name)
	{
		// Member names can't start with numbers or be reserved identifiers. Slowwwwwwwwww.
		if (isdigit(Name[0]) ||
			Name == "float" ||
			Name == "uint" ||
			Name == "int" ||
			Name == "HalfFloat" ||
			Name == "Vec4" ||
			Name == "uint32" ||
			Name == "uint16" ||
			Name == "uint8" ||
			Name == "RGBAColorRev" ||
			Name == "FRGBAColor")
			Name = "_" + Name;
	}

	bool IsPostLoadCallbackEnabled(const RTTI *Type)
	{
		if (Type->m_InfoType != RTTI::INFO_TYPE_CLASS)
			__debugbreak();

		for (auto& event : Type->ClassEventSubscriptions())
		{
			if (event.m_Type->GetSymbolName() == "MsgReadBinary")
				return true;
		}

		return false;
	}

	bool IsMemberNameDuplicated(const RTTI *Type, const RTTIMemberTypeInfo *MemberInfo)
	{
		if (Type->m_InfoType != RTTI::INFO_TYPE_CLASS)
			__debugbreak();

		for (auto& member : Type->ClassMembers())
		{
			if (&member == MemberInfo || member.IsSaveStateOnly())
				continue;

			if (!strcmp(member.m_Name, MemberInfo->m_Name))
				return true;
		}

		return false;
	}

	void BuildFullClassMemberLayout(const RTTI *Type, std::vector<SorterEntry>& Members, uint32_t Offset, bool TopLevel)
	{
		const char *activeCategory = "";

		for (auto& base : Type->ClassInheritance())
			BuildFullClassMemberLayout(base.m_Type, Members, Offset + base.m_Offset, false);

		for (auto& member : Type->ClassMembers())
		{
			if (!member.m_Type)
				activeCategory = member.m_Name;

			SorterEntry entry
			{
				.m_Type = &member,
				.m_Category = activeCategory,
				.m_Offset = member.m_Offset + Offset,
				.m_TopLevel = TopLevel,
			};

			Members.emplace_back(entry);
		}
	}

	std::vector<std::pair<const RTTIMemberTypeInfo *, const char *>> GetSortedClassMembers(const RTTI *Type)
	{
		// Nasty hack: I don't know how sorting order works with multiple properties at offset 0. Let the game determine it.
		std::vector<SorterEntry> sortedEntries;
		BuildFullClassMemberLayout(Type, sortedEntries, 0, true);

		auto sortCompare = [](const SorterEntry *A, const SorterEntry *B)
		{
			return A->m_Offset < B->m_Offset;
		};

		if (sortedEntries.size() > 1)
		{
			auto start = &sortedEntries.data()[0];
			auto end = &sortedEntries.data()[sortedEntries.size() - 1];
			uint32_t temp = 0;

			// Signature is valid across both games. I'm amazed. 9/19/2020.
			const static auto addr = XUtil::FindPattern(g_ModuleBase, g_ModuleSize, "48 89 6C 24 20 56 41 56 41 57 48 83 EC 20 48 8B 02 4D 8B F9 49 8B E8 48 8B F2 4C 8B F1 48 39 01 0F 83 56 01 00 00 45 69 11 0D 66 19 00 48 B8 39 8E E3 38 8E E3 38 0E");
			((void(__fastcall *)(SorterEntry **, SorterEntry **, bool(__fastcall *)(const SorterEntry *, const SorterEntry *), uint32_t *))(addr))(&start, &end, sortCompare, &temp);
		}

		// We only care about the top-level fields here
		std::vector<std::pair<const RTTIMemberTypeInfo *, const char *>> out;

		for (auto& entry : sortedEntries)
		{
			if (entry.m_TopLevel)
				out.emplace_back(entry.m_Type, entry.m_Category);
		}

		return out;
	}
}