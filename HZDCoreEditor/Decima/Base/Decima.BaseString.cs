using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Utility;

namespace Decima
{
    /// <remarks>
    /// File data format:
    /// UInt32  (+0) String length
    /// UInt32  (+4) Case sensitive CRC32-C hash with the most significant bit set to 0
    /// UInt8[] (+8) String data
    /// </remarks>
    [DebuggerDisplay("{ToString()}")]
    public class BaseString : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Value;

        public BaseString() : this("")
        {
        }

        public BaseString(string value)
        {
            Value = value;
        }

        public void Deserialize(BinaryReader reader)
        {
            uint readLength = reader.ReadUInt32() * sizeof(byte);

            if (readLength > reader.StreamRemainder())
                throw new Exception("String is out of bounds");

            if (readLength > 0)
            {
                uint hash = reader.ReadUInt32();
                byte[] data = reader.ReadBytesStrict(readLength);

                Value = Encoding.UTF8.GetString(data);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            byte[] data = Encoding.UTF8.GetBytes(Value);
            writer.Write((uint)data.Length);

            if (data.Length > 0)
            {
                writer.Write(CRC32C.Checksum(data) & ~0x80000000u);
                writer.Write(data);
            }
        }

        public void DeserializeStateObject(SaveState state)
        {
            Value = state.ReadIndexedString();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator BaseString(string value)
        {
            return new BaseString(value);
        }
        public static implicit operator string(BaseString value)
        {
            return value?.Value;
        }
    }
}