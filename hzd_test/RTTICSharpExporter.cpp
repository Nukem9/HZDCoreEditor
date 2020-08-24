#include "common.h"
#include "RTTICSharpExporter.h"

extern std::unordered_set<const RTTI *> AllRegisteredTypeInfo;

namespace RTTICSharpExporter
{
	void ExportAll(const char *Directory)
	{
		// Build a list of all {classes|enums}, sort by name, then dump to separate files
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

		// TODO: Split classes into separate files if they all reference a common base (i.e > 30 instances per)
		char outputFilePath[MAX_PATH];
		sprintf_s(outputFilePath, "%s\\%s.cs", Directory, "Decima.GameStructs");

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

		sprintf_s(outputFilePath, "%s\\%s.cs", Directory, "Decima.GameEnums");

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
			"using System.Runtime.InteropServices;\n"
			"\n"
			"namespace Decima\n"
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
			"\n"
			"    static partial class GameData\n"
			"    {\n";

		fputs(data, F);
	}

	void ExportFileFooter(FILE *F)
	{
		const char *data =
			"    }\n"
			"}\n";

		fputs(data, F);
	}

	void ExportRTTIEnum(FILE *F, const RTTI *Type)
	{
		// Attributes/decl
		fprintf(F, "[RTTI.Serializable(0x%llX, 0x%X)]\n", Type->GetCoreBinaryTypeId_UNSAFE(), Type->m_EnumUnderlyingTypeSize);
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

		size_t baseClassOffset = 0;
		bool postLoadCallback = IsPostLoadCallbackEnabled(Type);

		if (!inheritance.empty())
		{
			// TODO: Sort 'inheritance'?
			for (auto& base : inheritance)
				baseClassOffset = std::max<size_t>(baseClassOffset, base.m_Offset + base.m_Type->Class.m_Size);

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
		// public partial class AIAtmosphereBoxResource : Resource, RTTI.IExtraBinaryDataCallback
		// public partial class AIAtmosphereBoxResource : RTTI.IExtraBinaryDataCallback
		//
		char fullDecl[1024] = {};

		if (postLoadCallback)
			sprintf_s(fullDecl, "public partial class %s%s", Type->GetSymbolName().c_str(), inheritanceDecl);
		else
			sprintf_s(fullDecl, "public class %s%s", Type->GetSymbolName().c_str(), inheritanceDecl);

		//
		// Possible attributes:
		//
		// [StructLayout(LayoutKind.Sequential)]
		// [RTTI.Serializable(0xDC3D43D192F22E9B, 0x60)]
		//
		fprintf(F, "[StructLayout(LayoutKind.Sequential)]\n");
		fprintf(F, "[RTTI.Serializable(0x%llX, 0x%X)]\n", Type->GetCoreBinaryTypeId_UNSAFE(), Type->Class.m_Size);
		fprintf(F, "%s\n{\n", fullDecl);

		// Insert fake members from base classes, skipping the first one
		for (size_t i = 1; i < inheritance.size(); i++)
		{
			if (IsBaseClassSuperfluous(inheritance[i].m_Type))
				continue;

			auto name = inheritance[i].m_Type->GetSymbolName();
			fprintf(F, "\t%s base_%s;\n", name.c_str(), name.c_str());
		}

		// Build a list of real members and sort them by offset
		const char *activeCategory = "";
		std::vector<std::pair<const RTTIMemberTypeInfo *, const char *>> members;

		for (auto& member : Type->ClassMembers())
		{
			if (!member.m_Type)
			{
				// Internal marker
				activeCategory = member.m_Name;
			}
			else if (!member.IgnoreBinarySerialization())
			{
				// Regular variable
				members.emplace_back(&member, activeCategory);
			}
		}

		std::stable_sort(members.begin(), members.end(), [](const auto& A, const auto& B)
		{
			return A.first->m_Offset < B.first->m_Offset;
		});

		// Insert real members
		for (auto& [member, category] : members)
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

			if (member->m_Offset == 0 && member->m_Offset < baseClassOffset)
			{
				// Certain classes (ex. EntityResource {Lockable, ZoomLockable}) have variables declared at offset 0 WITH parent types. That's not possible in C# or
				// C++. How did their reflection system break this? Anyway, I have to force a specific order with stupid attributes now.
				fprintf(F, "\t[RTTI.BrokenReflectionOffset(0x%X)]\n", member->m_Offset);
			}

			fprintf(F, "\t%s %s;\n", typeName.c_str(), memberName.c_str());
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
			if (!member.IgnoreBinarySerialization())
				return false;
		}

		return true;
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
			if (&member == MemberInfo || member.IgnoreBinarySerialization())
				continue;

			if (!strcmp(member.m_Name, MemberInfo->m_Name))
				return true;
		}

		return false;
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