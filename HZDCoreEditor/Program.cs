using Decima;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace HZDCoreEditor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*
            Needs to be moved to its own unit test project

            RTTI.SetGameMode(GameType.DS);
            TestsDS.DecodeReserializeQuickCoreFilesTest();
            TestsDS.DecodeReserializeAllCoreFilesTest();
            TestsDS.DecodeQuickCoreFilesTest();
            TestsDS.DecodeAllCoreFilesTest();
            TestsDS.DecodeArchivesBenchmarkTest();
            TestsDS.DecodeQuickArchivesTest();

            RTTI.SetGameMode(GameType.HZD);
            TestsHZD.DecodeSavesTest();
            TestsHZD.DecodeReserializeQuickCoreFilesTest();
            TestsHZD.DecodeReserializeAllCoreFilesTest();
            TestsHZD.DecodeQuickCoreFilesTest();
            TestsHZD.DecodeAllCoreFilesTest();
            TestsHZD.DecodeQuickArchivesTest();
            TestsHZD.DecodeAllArchivesTest();
            */

            /*
            RTTI.SetGameMode(GameType.HZD);
            ExtractHZDLocalization();
            ExtractHZDAudio();
            */
        }

        static void ExtractHZDLocalization()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\localized\", "*.core", SearchOption.AllDirectories);
            var allStrings = new List<string>();

            foreach (string file in files)
            {
                Console.WriteLine(file);

                allStrings.Add("\n");
                allStrings.Add(file);

                var objects = CoreBinary.Load(file);

                foreach (var obj in objects)
                {
                    if (obj is Decima.HZD.LocalizedTextResource asResource)
                        allStrings.Add(asResource.GetStringForLanguage(Decima.HZD.ELanguage.English));
                }
            }

            File.WriteAllLines(@"C:\text_data_dump.txt", allStrings);
        }

        static void ExtractHZDAudio()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\", "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var coreObjects = CoreBinary.Load(file);

                foreach (var obj in coreObjects)
                {
                    var wave = obj as Decima.HZD.WaveResource;

                    if (wave == null)
                        continue;

                    if (wave.IsStreaming)
                        continue;

                    using (var ms = new System.IO.MemoryStream(wave.WaveData.ToArray()))
                    {
                        RawSourceWaveStream rs = null;

                        if (wave.Encoding == Decima.HZD.EWaveDataEncoding.MP3)
                            rs = new RawSourceWaveStream(ms, new Mp3WaveFormat(wave.SampleRate, wave.ChannelCount, wave.FrameSize, (int)wave.BitsPerSecond));
                        else if (wave.Encoding == Decima.HZD.EWaveDataEncoding.PCM)
                            rs = new RawSourceWaveStream(ms, new WaveFormat(wave.SampleRate, 16, wave.ChannelCount));// This is wrong

                        string outFile = Path.Combine(Path.GetDirectoryName(file), wave.Name.Value + ".wav");

                        if (rs != null)
                            WaveFileWriter.CreateWaveFile(outFile, rs);
                    }
                }
            }
        }
    }
}
