using System.IO;

namespace HZDCoreEditor.Util
{
    internal static class BinaryStreamExtensions
    {
        public static long StreamLength(this BinaryReader reader)
        {
            return reader.BaseStream.Length;
        }

        public static long StreamRemainder(this BinaryReader reader)
        {
            return reader.BaseStream.Length - reader.BaseStream.Position;
        }

        public static byte[] ReadBytes(this BinaryReader reader, uint count)
        {
            return reader.ReadBytes((int)count);
        }

        public static byte[] ReadBytesStrict(this BinaryReader reader, int count)
        {
            byte[] data = reader.ReadBytes(count);

            if (data.Length != count)
                throw new EndOfStreamException();

            return data;
        }

        public static byte[] ReadBytesStrict(this BinaryReader reader, uint count)
        {
            return reader.ReadBytesStrict((int)count);
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
