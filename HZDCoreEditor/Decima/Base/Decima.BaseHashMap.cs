using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Decima
{
    /// <remarks>
    /// File data format:
    /// UInt32        (+0) Item count
    /// <UInt32, T>[] (+4) (Optional) Array items
    /// </remarks>
    [DebuggerDisplay("{Value}")]
    public class BaseHashMap<T> : Dictionary<uint, T>, RTTI.ISerializable, RTTI.ISaveSerializable
    {
        public void Deserialize(BinaryReader reader)
        {
            uint itemCount = reader.ReadUInt32();

            if (itemCount > reader.StreamRemainder())
                throw new Exception("HashMap item count is out of bounds");

            for (uint i = 0; i < itemCount; i++)
            {
                uint entryHash = reader.ReadUInt32();
                var newObj = RTTI.DeserializeType<T>(reader);

                // TODO: is entryHash actually a hash?
                Add(entryHash, newObj);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)Count);

            foreach (var element in this)
            {
                writer.Write(element.Key);
                RTTI.SerializeType(writer, element.Value);
            }
        }
        public void DeserializeStateObject(SaveState state) => throw new NotImplementedException();
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}