using System;
using System.IO;

namespace Decima
{
    static partial class RTTI
    {
        /// <summary>
        /// Describes a class, struct, or enum that is serialized as Core binary data using reflection
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
        public class SerializableAttribute : Attribute
        {
            public readonly ulong BinaryTypeId;
            public readonly GameType Game;

            public SerializableAttribute(ulong binaryTypeId, GameType game)
            {
                BinaryTypeId = binaryTypeId;
                Game = game;
            }
        }

        /// <summary>
        /// Describes a class member that is serialized as Core binary data
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class MemberAttribute : Attribute
        {
            public readonly uint Order;
            public readonly uint RuntimeOffset;
            public readonly string Category;
            public readonly bool SaveStateOnly;

            public MemberAttribute(uint order, uint runtimeOffset, string category = "", bool saveStateOnly = false)
            {
                Order = order;
                RuntimeOffset = runtimeOffset;
                Category = category;
                SaveStateOnly = saveStateOnly;
            }

            public MemberAttribute(uint order, uint runtimeOffset, bool saveStateOnly) : this(order, runtimeOffset, "", saveStateOnly)
            {
            }
        }

        /// <summary>
        /// Describes a class member that is emulating C++ multiple base class inheritance
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class BaseClassAttribute : MemberAttribute
        {
            public BaseClassAttribute(uint runtimeOffset) : base(0, runtimeOffset, null)
            {
            }
        }

        /// <summary>
        /// Interface used for classes that manually encode/decode all of their fields
        /// </summary>
        public interface ISerializable
        {
            public void Deserialize(BinaryReader reader) => throw new NotImplementedException();

            public void Serialize(BinaryWriter writer) => throw new NotImplementedException();
        }

        /// <summary>
        /// Interface identical to <see cref="ISerializable"/>, but used exclusively for save game data
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
            public void DeserializeStateObject(SaveState state) => throw new NotImplementedException();

            public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
        }

        /// <summary>
        /// Interface used for classes that have data appended after the structure fields (Equivalent to MsgReadBinary)
        /// </summary>
        public interface IExtraBinaryDataCallback
        {
            public void DeserializeExtraData(BinaryReader reader)
            {
            }

            public void SerializeExtraData(BinaryWriter writer) => throw new NotImplementedException();
        }
    }
}