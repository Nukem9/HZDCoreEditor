using System;

namespace Decima
{
    public static partial class RTTI
    {
        /// <summary>
        /// Describes a class, struct, or enum that is serialized as Core binary data using reflection.
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
        /// Describes a class member that is serialized as Core binary data using reflection.
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
        /// Describes a class member that is emulating C++ multiple class inheritance.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class BaseClassAttribute : MemberAttribute
        {
            public BaseClassAttribute(uint runtimeOffset) : base(0, runtimeOffset, null)
            {
            }
        }
    }
}