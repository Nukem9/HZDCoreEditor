using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Utility;

namespace Decima
{
    /// <remarks>
    /// File data format:
    /// UInt32   (+0) String length
    /// UInt16[] (+4) String data
    /// </remarks>
    [DebuggerDisplay("{ToString()}")]
    public class BaseWString : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Value;

        public void Deserialize(BinaryReader reader)
        {
            uint readLength = reader.ReadUInt32() * sizeof(ushort);

            if (readLength > reader.StreamRemainder())
                throw new Exception("String is out of bounds");

            if (readLength > 0)
            {
                byte[] data = reader.ReadBytesStrict(readLength);

                Value = Encoding.Unicode.GetString(data);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((uint)Value.Length);

            if (Value.Length > 0)
                writer.Write(Encoding.Unicode.GetBytes(Value));
        }

        public void DeserializeStateObject(SaveState state)
        {
            Value = state.ReadIndexedWideString();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}