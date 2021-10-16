using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace Decima
{
    /// <remarks>
    /// File data format:
    /// UInt32 (+0) Item count
    /// T[]    (+4) (Optional) Array items
    /// </remarks>
    public class BaseArray<T> : List<T>, RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public BaseArray() : base()
        {
        }

        public BaseArray(int capacity) : base(capacity)
        {
        }

        public void Deserialize(BinaryReader reader)
        {
            uint itemCount = reader.ReadUInt32();

            if (itemCount > reader.StreamRemainder())
                throw new Exception("Array item count is out of bounds");

            if (itemCount == 0)
                return;

            if (typeof(T) == typeof(int))
            {
                var bytes = reader.ReadBytesStrict(itemCount * sizeof(int));
                var ints = new int[itemCount];
                Buffer.BlockCopy(bytes, 0, ints, 0, bytes.Length);
                (this as List<int>).AddRange(ints);
            }
            else if (typeof(T) == typeof(byte))
            {
                // Avoid wasting time on large arrays
                (this as List<byte>).AddRange(reader.ReadBytesStrict(itemCount));
            }
            else
            {
                for (uint i = 0; i < itemCount; i++)
                {
                    var newObj = RTTI.DeserializeType<T>(reader);

                    Add(newObj);
                }
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)Count);

            foreach (var element in this)
                RTTI.SerializeType(writer, element);
        }

        public void DeserializeStateObject(SaveState state)
        {
            int itemCount = state.ReadVariableLengthOffset();

            for (int i = 0; i < itemCount; i++)
            {
                var newObj = state.DeserializeType<T>();

                Add(newObj);
            }
        }

        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}