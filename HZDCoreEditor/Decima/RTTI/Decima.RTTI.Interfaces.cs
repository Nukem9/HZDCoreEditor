using System.IO;

namespace Decima
{
    public static partial class RTTI
    {
        /// <summary>
        /// Interface used for classes that manually encode/decode all of their fields.
        /// </summary>
        public interface ISerializable
        {
            void Deserialize(BinaryReader reader);

            void Serialize(BinaryWriter writer);
        }

        /// <summary>
        /// Interface used for classes that manually encode/decode all of their fields, but used exclusively for save game data.
        /// </summary>
        /// <remarks>
        /// [X] Ref<>, StreamingRef<>, UUIDRef<>, Ptr<>, WeakPtr<>, Array<>, HashMap<>, HashSet<>, String, WString
        /// [X] int, uint, int32, uint32
        /// [X] GGUUID
        /// [ ] RGBAColor
        /// [ ] Mat44
        /// [X] WorldTransform
        /// [X] Vec2
        /// [X] Vec3
        /// [ ] Vec4
        /// [ ] Quat
        /// [ ] Mat34
        /// [ ] RotMatrix
        /// [X] WorldPosition
        /// [X] IVec2
        /// [ ] IVec3
        /// [ ] FArc
        /// [ ] FSize
        /// [ ] IRect
        /// [ ] FRect
        /// [ ] FRGBColor
        /// [ ] FRGBAColor
        /// 
        /// [X] CountdownTimerManager
        /// [X] FactDatabase
        /// [X] GeneratedQuestSave
        /// [X] MenuBadgeManager
        /// [X] QuestSystem
        /// [ ] ScriptMessageQueue
        /// [ ] StateObject
        /// [X] Story
        /// [X] WorldEncounterManager
        /// 
        /// [X] QuestSave - Bug. See comments.
        /// [X] uint128 - Non-native type in C#
        /// </remarks>
        public interface ISaveSerializable
        {
            void DeserializeStateObject(SaveState state);

            void SerializeStateObject(SaveState state);
        }

        /// <summary>
        /// Interface used for classes that have data appended after their fields. Equivalent to MsgReadBinary.
        /// </summary>
        public interface IExtraBinaryDataCallback
        {
            void DeserializeExtraData(BinaryReader reader);

            void SerializeExtraData(BinaryWriter writer);
        }
    }
}