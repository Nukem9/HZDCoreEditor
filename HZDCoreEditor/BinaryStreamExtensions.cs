using System.IO;

namespace BinaryStreamExtensions
{
    public static class IShouldntHaveToAddThese
    {
        public static ulong StreamLength(this BinaryReader reader)
        {
            return (ulong)reader.BaseStream.Length;
        }

        public static byte[] ReadBytes(this BinaryReader reader, uint count)
        {
            return reader.ReadBytes((int)count);
        }

        public static byte[] ReadBytesStrict(this BinaryReader reader, uint count)
        {
            byte[] data = reader.ReadBytes((int)count);

            if (data.Length != count)
                throw new EndOfStreamException();

            return data;
        }

        public static bool ReadBooleanStrict(this BinaryReader reader)
        {
            byte value = reader.ReadByte();

            return value switch
            {
                0 => false,
                1 => true,
                _ => throw new InvalidDataException(),
            };
        }
    }
}
