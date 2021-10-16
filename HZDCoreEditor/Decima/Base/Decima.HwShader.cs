using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace Decima
{
    public class HwShader
    {
        // Identical impl to HZD
        BaseGGUUID ResourceGUID;
        public uint Unknown1;
        public BaseProgramTypeMask TypeMask;
        public uint Unknown2;
        public List<ProgramEntry> Programs;
        public byte[] RootSignatureData;

        public class ProgramEntry
        {
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public BaseProgramType HlslProgramType;
            public uint HlslModelVersion;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public byte[] HlslData;

            public void ToData(BinaryWriter writer, GameType gameType)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write((uint)HlslProgramType);

                if (gameType == GameType.DS)
                    writer.Write(HlslModelVersion);

                writer.Write(Unknown6);
                writer.Write(Unknown7);
                writer.Write(Unknown8);
                writer.Write(Unknown9);
                writer.Write(Unknown10);
                writer.Write(HlslData.Length);
                writer.Write(HlslData);
            }

            public static ProgramEntry FromData(BinaryReader reader, GameType gameType)
            {
                var x = new ProgramEntry();

                x.Unknown1 = reader.ReadUInt32();
                x.Unknown2 = reader.ReadUInt32();
                x.Unknown3 = reader.ReadUInt32();
                x.Unknown4 = reader.ReadUInt32();
                x.Unknown5 = reader.ReadUInt32();
                x.HlslProgramType = (BaseProgramType)reader.ReadUInt32();

                if (gameType == GameType.DS)
                    x.HlslModelVersion = reader.ReadUInt32();
                else
                    x.HlslModelVersion = 5;

                x.Unknown6 = reader.ReadUInt32();
                x.Unknown7 = reader.ReadUInt32();
                x.Unknown8 = reader.ReadUInt32();
                x.Unknown9 = reader.ReadUInt32();
                x.Unknown10 = reader.ReadUInt32();

                uint shaderDataLength = reader.ReadUInt32();
                x.HlslData = reader.ReadBytesStrict(shaderDataLength);

                return x;
            }
        }

        public void ToData(BinaryWriter writer, GameType gameType)
        {
            // Write to a separate memory stream buffer in order to get the block length (shaderDataLength)
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            bw.Write(Unknown1);
            bw.Write((uint)TypeMask);
            bw.Write(Unknown2);

            if (gameType == GameType.DS)
                bw.Write((uint)Programs.Count);

            foreach (var entry in Programs)
                entry.ToData(bw, gameType);

            bw.Write((uint)RootSignatureData.Length);
            bw.Write(RootSignatureData);

            writer.Write((uint)ms.Length);
            ResourceGUID.ToData(writer);
            ms.WriteTo(writer.BaseStream);
        }

        public static HwShader FromData(BinaryReader reader, GameType gameType)
        {
            var x = new HwShader();

            // shaderDataLength is discarded
            uint shaderDataLength = reader.ReadUInt32();
            x.ResourceGUID = new BaseGGUUID().FromData(reader);

            x.Unknown1 = reader.ReadUInt32();
            x.TypeMask = (BaseProgramTypeMask)reader.ReadUInt32();
            x.Unknown2 = reader.ReadUInt32();// Related to type mask

            // Horizon Zero Dawn has it hardcoded for some reason
            uint shaderEntryCount = gameType switch
            {
                GameType.DS => reader.ReadUInt32(),
                GameType.HZD => 4,
                _ => throw new NotImplementedException(),
            };

            x.Programs = new List<ProgramEntry>((int)shaderEntryCount);

            for (uint i = 0; i < shaderEntryCount; i++)
                x.Programs.Add(ProgramEntry.FromData(reader, gameType));

            uint rootSignatureDataLength = reader.ReadUInt32();
            x.RootSignatureData = reader.ReadBytesStrict(rootSignatureDataLength);

            return x;
        }
    }
}