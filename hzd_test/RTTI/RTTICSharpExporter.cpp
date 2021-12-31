#include <windows.h>
#include <algorithm>

#include "RTTICSharpExporter.h"

RTTICSharpExporter::RTTICSharpExporter(const std::unordered_set<const HRZ::RTTI *>& Types, const std::string_view GameTypePrefix) : m_Types(Types.begin(), Types.end()), m_GameTypePrefix(GameTypePrefix)
{
	// Always sort by name during export
	std::sort(m_Types.begin(), m_Types.end(), [](const HRZ::RTTI *A, const HRZ::RTTI *B)
	{
		return A->GetSymbolName() < B->GetSymbolName();
	});
}

void RTTICSharpExporter::ExportAll(const std::string_view Directory)
{
	CreateDirectoryA(Directory.data(), nullptr);

	// Dump these types into their own separate files
	static const char *separatedTypes[] =
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

	// Structures/classes
	// TODO: Split classes into separate files if they all reference a common base (i.e > 30 instances per). This is done manually right now.
	auto tryExportSeparateFile = [&](const HRZ::RTTI *Type)
	{
		const auto typeName = Type->GetSymbolName();

		for (auto name : separatedTypes)
		{
			if (typeName == name)
			{
				auto filePath = std::format("{0:}\\Decima.{1:}.{2:}.cs", Directory, m_GameTypePrefix, name);
				auto oldHandle = m_FileHandle;

				if (fopen_s(&m_FileHandle, filePath.c_str(), "w") == 0)
				{
					ExportFileHeader();
					ExportRTTIClass(Type->AsClass());
					ExportFileFooter();
					fclose(m_FileHandle);
				}

				m_FileHandle = oldHandle;
				return true;
			}
		}

		return false;
	};

	auto filePath = std::format("{0:}\\Decima.{1:}.AllStructs.cs", Directory, m_GameTypePrefix);

	if (fopen_s(&m_FileHandle, filePath.c_str(), "w") == 0)
	{
		ExportFileHeader();

		for (auto& type : m_Types)
		{
			auto asClass = type->AsClass();

			if (!asClass)
				continue;

			if (!tryExportSeparateFile(asClass))
				ExportRTTIClass(asClass);
		}

		ExportFileFooter();
		fclose(m_FileHandle);
	}

	// Enums
	filePath = std::format("{0:}\\Decima.{1:}.AllEnums.cs", Directory, m_GameTypePrefix);

	if (fopen_s(&m_FileHandle, filePath.c_str(), "w") == 0)
	{
		ExportFileHeader();

		for (auto& type : m_Types)
		{
			if (auto e = type->AsEnum(); e)
				ExportRTTIEnum(e);
		}

		ExportFileFooter();
		fclose(m_FileHandle);
	}
}

void RTTICSharpExporter::ExportFileHeader()
{
	const char *data =
		"\n"
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

	Print("#pragma warning disable CS0649 // warning CS0649: 'member' is never assigned to, and will always have its default value 'value'.\n");
	Print("#pragma warning disable CS0108 // warning CS0108: 'class' hides inherited member 'member'. Use the new keyword if hiding was intended.\n");
	Print("\n");
	Print("namespace Decima.{0:}\n{{", m_GameTypePrefix);
	Print(data);
}

void RTTICSharpExporter::ExportFileFooter()
{
	const char *data =
		"}}\n";

	Print(data);
}

void RTTICSharpExporter::ExportRTTIEnum(const HRZ::RTTIEnum *Type)
{
	// Attributes/decl
	Print("[RTTI.Serializable(0x{0:X}, GameType.{1:})]\n", Type->GetCoreBinaryTypeId(), m_GameTypePrefix);
	Print("public enum {0:} : {1:}\n{{\n", Type->GetSymbolName(), EnumTypeToString(Type));

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

		Print("\t{0:} = {1:},\n", filteredName, member.m_Value);
		index++;
	}

	Print("}}\n\n");
}

void RTTICSharpExporter::ExportRTTIClass(const HRZ::RTTIClass *Type)
{
	// C# doesn't support multiple base classes, so pick one based on the order in RTTI data and treat the others as members (manual composition)
	auto inheritance = Type->ClassInheritance();
	std::string inheritanceDecl;

	if (!inheritance.empty())
	{
		// TODO: Sort 'inheritance'?
		inheritanceDecl = std::format(" : {0:}", inheritance[0].m_Type->GetSymbolName());
	}

	if (Type->HasPostLoadCallback())
	{
		if (!inheritanceDecl.empty())
			inheritanceDecl += ", RTTI.IExtraBinaryDataCallback";
		else
			inheritanceDecl += " : RTTI.IExtraBinaryDataCallback";
	}

	//
	// Possible declarations:
	//
	// public class AIAtmosphereBoxResource
	// public class AIAtmosphereBoxResource : Resource
	// public class AIAtmosphereBoxResource : Resource, RTTI.IExtraBinaryDataCallback
	// public class AIAtmosphereBoxResource : RTTI.IExtraBinaryDataCallback
	//
	auto fullDecl = std::format("public class {0:}{1:}", Type->GetSymbolName(), inheritanceDecl);

	//
	// Possible attributes:
	//
	// [RTTI.Serializable(0xDC3D43D192F22E9B, GameType.HZD)]
	//
	Print("[RTTI.Serializable(0x{0:X}, GameType.{1:})]\n", Type->GetCoreBinaryTypeId(), m_GameTypePrefix);
	Print("{0:}\n{{\n", fullDecl);

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
		if (IsBaseClassSuperfluous(inheritance[i].m_Type))
			continue;

		Print("\t[RTTI.BaseClass(0x{0:X})] public {1:} @{1:};\n", inheritance[i].m_Offset, inheritance[i].m_Type->GetSymbolName());
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

		std::string attributeDecl;

		if (strlen(category) > 0)
			attributeDecl = std::format("[RTTI.Member({0:}, 0x{1:X}, \"{2:}\"", index + declOrder, member->m_Offset, category);
		else
			attributeDecl = std::format("[RTTI.Member({0:}, 0x{1:X}", index + declOrder, member->m_Offset);

		if (member->IsSaveStateOnly())
			attributeDecl += ", true)]";
		else
			attributeDecl += ")]";

		Print("\t{0:} public {1:} {2:};\n", attributeDecl, typeName, memberName);
	}

	Print("}}\n\n");
}

bool RTTICSharpExporter::IsBaseClassSuperfluous(const HRZ::RTTIClass *Type)
{
	// Returns true if this type and all subtypes have no members listed in the binary format
	for (auto& member : Type->ClassMembers())
	{
		if (!member.IsGroupMarker())
			return false;
	}

	for (auto& base : Type->ClassInheritance())
	{
		if (!IsBaseClassSuperfluous(base.m_Type))
			return false;
	}

	return true;
}

bool RTTICSharpExporter::IsMemberNameDuplicated(const HRZ::RTTIClass *Type, const HRZ::RTTIClass::MemberEntry *MemberInfo)
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

std::string_view RTTICSharpExporter::EnumTypeToString(const HRZ::RTTIEnum *Type)
{
	switch (Type->m_EnumUnderlyingTypeSize)
	{
	case 1: return "int8";
	case 2: return "int16";
	case 4: return "int32";
	case 8: return "int64";
	}

	__debugbreak();
	return "<INVALID>";
}

void RTTICSharpExporter::FilterMemberNameString(std::string& Name)
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