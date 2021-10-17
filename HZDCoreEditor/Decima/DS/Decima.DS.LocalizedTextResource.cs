using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decima.DS
{
    [RTTI.Serializable(0x31BE502435317445, GameType.DS)]
    public class LocalizedTextResource : Resource, RTTI.IExtraBinaryDataCallback
    {
        private const int LanguageCount = (int)ELanguage.Hungarian;

        public List<LocalizationEntry> Entries;

        public class LocalizationEntry
        {
            public string Text;
            public string Notes;
            public byte Flags;
        }

        public void DeserializeExtraData(BinaryReader reader)
        {
            Entries = new List<LocalizationEntry>(LanguageCount);

            for (int i = 0; i < LanguageCount; i++)
            {
                var entry = new LocalizationEntry();

                ushort textLength = reader.ReadUInt16();
                entry.Text = Encoding.UTF8.GetString(reader.ReadBytesStrict(textLength));
                ushort noteLength = reader.ReadUInt16();
                entry.Notes = Encoding.UTF8.GetString(reader.ReadBytesStrict(noteLength));
                entry.Flags = reader.ReadByte();

                Entries.Add(entry);
            }
        }

        public void SerializeExtraData(BinaryWriter writer)
        {
            foreach (var entry in Entries)
            {
                var textBytes = Encoding.UTF8.GetBytes(entry.Text);
                var notesBytes = Encoding.UTF8.GetBytes(entry.Notes);

                writer.Write((ushort)textBytes.Length);
                writer.Write(textBytes);
                writer.Write((ushort)notesBytes.Length);
                writer.Write(notesBytes);
                writer.Write(entry.Flags);
            }
        }

        public string GetStringForLanguage(ELanguage language)
        {
            if (language == ELanguage.Unknown)
                throw new ArgumentException("Invalid language", nameof(language));

            return Entries[(int)language - 1].Text;
        }
    }
}