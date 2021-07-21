#include "common.h"
#include "RTTICSharpExporter.h"

extern std::unordered_set<const HRZ::RTTI *> AllRegisteredTypeInfo;

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
			"AnimationStreamingBlockResource",
			"AnimationStreamingEventResource",
			"BuddyManager",
			"CollectableManager",
			"CountdownTimerManager",
			"DataBufferResource",
			"EntityManagerGame",
			"ExplorationSystem",
			"FactDatabase",
			"GameModule",
			"GameSettings",
			"GeneratedQuestSave",
			"GGUUID",
			"IndexArrayResource",
			"IVec2",
			"LocalizedSimpleSoundResource",
			"LocalizedTextResource",
			"LocationMarkerManager",
			"MapZoneManager",
			"MenuBadgeManager",
			"MissionSettings",
			"MorphemeAnimationResource",
			"MusicResource",
			"PhysicsRagdollResource",
			"PhysicsShapeResource",
			"PickUpDatabaseGame",
			"PlayerGame",
			"Pose",
			"QuestSave",
			"QuestSystem",
			"RegularSkinnedMeshResource",
			"RotMatrix",
			"SceneManagerGame",
			"ShaderResource",
			"StaticMeshResource",
			"Story",
			"StreamingStrategyInstance",
			"StreamingStrategyManagerGame",
			"Texture",
			"TextureList",
			"TileBasedStreamingStrategyInstance",
			"UITexture",
			"Vec2",
			"Vec3",
			"VertexArrayResource",
			"WaveResource",
			"WeatherSystem",
			"WorldEncounterManager",
			"WorldPosition",
			"WorldState",
			"WorldTransform",
			"WwiseBankResource",
			"WwiseWemLocalizedResource",
			"WwiseWemResource",
		};

		sortedTypes.erase(std::remove_if(sortedTypes.begin(), sortedTypes.end(), [Directory, separatedTypes](const RTTI *Type)
		{
			for (auto name : separatedTypes)
			{
				if (Type->GetSymbolName() == name)
				{
					char outputFilePath[MAX_PATH];
					sprintf_s(outputFilePath, "%s\\Decima.%s.%s.cs", Directory, g_GamePrefix, name);

					if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
					{
						ExportFileHeader(f);
						ExportRTTIClass(f, Type->AsClass());
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
		sprintf_s(outputFilePath, "%s\\Decima.%s.AllStructs.cs", Directory, g_GamePrefix);

		if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
		{
			ExportFileHeader(f);

			for (auto& type : sortedTypes)
			{
				if (type->AsClass())
					ExportRTTIClass(f, type->AsClass());
			}

			ExportFileFooter(f);
			fclose(f);
		}

		sprintf_s(outputFilePath, "%s\\Decima.%s.AllEnums.cs", Directory, g_GamePrefix);

		if (FILE *f; fopen_s(&f, outputFilePath, "w") == 0)
		{
			ExportFileHeader(f);

			for (auto& type : sortedTypes)
			{
				if (type->AsEnum())
					ExportRTTIEnum(f, type->AsEnum());
			}

			ExportFileFooter(f);
			fclose(f);
		}
	}

	void ExportFileHeader(FILE *F)
	{
		const char *data =
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
			"    using ucs4 = System.Int32;\n"
			"\n"
			"    using HalfFloat = System.UInt16;\n"
			"    using LinearGainFloat = System.Single;\n"
			"    using MusicTime = System.UInt64;\n"
			"\n"
			"    using MaterialType = System.UInt16;\n"
			"    using AnimationNodeID = System.UInt16;\n"
			"    using AnimationTagID = System.UInt32;\n"
			"    using AnimationSet = System.UInt32;\n"
			"    using AnimationStateID = System.UInt32;\n"
			"    using AnimationEventID = System.UInt32;\n"
			"    using PhysicsCollisionFilterInfo = System.UInt32;\n"
			"\n";

		fputs("#pragma warning disable CS0649 // warning CS0649: 'member' is never assigned to, and will always have its default value 'value'.\n", F);
		fputs("#pragma warning disable CS0108 // warning CS0108: 'class' hides inherited member 'member'. Use the new keyword if hiding was intended.\n", F);
		fputs("\n", F);
		fprintf(F, "namespace Decima.%s\n", g_GamePrefix);
		fputs(data, F);
	}

	void ExportFileFooter(FILE *F)
	{
		const char *data =
			"}\n";

		fputs(data, F);
	}

	void ExportRTTIEnum(FILE *F, const RTTIEnum *Type)
	{
		// Attributes/decl
		fprintf(F, "[RTTI.Serializable(0x%llX, GameType.%s)]\n", Type->GetCoreBinaryTypeId(), g_GamePrefix);
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

	void ExportRTTIClass(FILE *F, const RTTIClass *Type)
	{
		// C# doesn't support multiple base classes, so pick one based on the order in RTTI data and treat the others as members (manual composition)
		char inheritanceDecl[1024] = {};
		auto inheritance = Type->ClassInheritance();
		bool postLoadCallback = Type->IsPostLoadCallbackEnabled();

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
		// [RTTI.Serializable(0xDC3D43D192F22E9B, GameType.HZD)]
		//
		fprintf(F, "[RTTI.Serializable(0x%llX, GameType.%s)]\n", Type->GetCoreBinaryTypeId(), g_GamePrefix);
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
		size_t index = 0;

		// Insert fake members from base classes, skipping the first one
		for (size_t i = 1; i < inheritance.size(); i++)
		{
			auto name = inheritance[i].m_Type->GetSymbolName();

			if (IsBaseClassSuperfluous(inheritance[i].m_Type->AsClass()))
				continue;

			fprintf(F, "\t[RTTI.BaseClass(0x%X)] public %s @%s;\n", inheritance[i].m_Offset, name.c_str(), name.c_str());
			index++;
		}

		// Insert real members sorted by offset
		auto members = Type->GetCategorizedClassMembers();

		for (auto& [member, category, declOrder] : members)
		{
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
				sprintf_s(attributeDecl, "[RTTI.Member(%llu, 0x%X, \"%s\"", index + declOrder, member->m_Offset, category);
			else
				sprintf_s(attributeDecl, "[RTTI.Member(%llu, 0x%X", index + declOrder, member->m_Offset);

			if (member->IsSaveStateOnly())
				strcat_s(attributeDecl, ", true)]");
			else
				strcat_s(attributeDecl, ")]");

			fprintf(F, "\t%s public %s %s;\n", attributeDecl, typeName.c_str(), memberName.c_str());
		}

		fprintf(F, "}\n\n");
	}

	bool IsBaseClassSuperfluous(const RTTIClass *Type)
	{
		// Returns true if this type and all subtypes have no members listed in the binary format
		for (auto& base : Type->ClassInheritance())
		{
			if (!IsBaseClassSuperfluous(base.m_Type->AsClass()))
				return false;
		}

		for (auto& member : Type->ClassMembers())
		{
			if (!member.IsGroupMarker())
				return false;
		}

		return true;
	}

	bool IsMemberNameDuplicated(const RTTIClass *Type, const RTTIClass::MemberEntry *MemberInfo)
	{
		for (auto& member : Type->ClassMembers())
		{
			if (&member == MemberInfo || member.IsGroupMarker())
				continue;

			if (!strcmp(member.m_Name, MemberInfo->m_Name))
				return true;
		}

		return false;
	}

	const char *EnumTypeToString(const RTTIEnum *Type)
	{
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
}