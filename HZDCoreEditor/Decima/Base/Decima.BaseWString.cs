using HZDCoreEditor.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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
            byte[] data = Encoding.Unicode.GetBytes(Value);
            writer.Write((uint)(data.Length / sizeof(ushort)));

            if (data.Length > 0)
                writer.Write(data);
        }

        public void DeserializeStateObject(SaveState state)
        {
            Value = state.ReadIndexedWideString();
        }
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();

        public override string ToString()
        {
            return Value;
        }
    }
}