using Decima;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace HZDCoreEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            DecodeAudioTest();
            return;

            var testFiles = new string[]
            {
                /*@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\effects\dlc1\weapons\firebreather\wav\fire_loop_flames_01_m.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\models\building_blocks\nora\dressing\components\dressing_b125_c006_resource.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\characters\models\humanoid_player.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\telemetry\designtelemetry.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\worlddata\worlddatapacking.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\weapons\heavy_weapons\heavy_railgun_cables.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\collectables\collectable_datapoint\datapoint_world.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\shops\shops.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\levels\quests.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\levels\game.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\levels\worlds\world\leveldata\terrain.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\weather\defaultweathersystem.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\climates\regions\faro\faro_master_climate.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\armor\outfits\outfits.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\models\weapons\raptordisc_playerversion\model\model.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\loadouts\loadout.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\levels\worlds\world\quests\mainquests\mq15_themountainthatfell_files\mq15_quest.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\characters\models\humanoid_civilian.core",
                @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\system\waves\white_noise_0dbfs.core",*/
            };

            foreach (string file in testFiles)
            {
                var objects = Decima.CoreBinary.Load(file, false);

                Console.WriteLine(file);
            }
        }

        static void DecodeLocalizationTest()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\localized\sentences\", "*", SearchOption.AllDirectories);
            var allStrings = new List<string>();

            foreach (string file in files)
            {
                Console.WriteLine(file);

                allStrings.Add("\n");
                allStrings.Add(file);

                try
                {
                    var objects = Decima.CoreBinary.Load(file, false);

                    foreach (var obj in objects)
                    {
                        if (obj is GameData.LocalizedTextResource asResource)
                            allStrings.Add(asResource.XX_Text);
                    }
                }
                catch (Exception)
                {
                    allStrings.Add("!!! File was skipped due to invalid data !!!");
                }
            }

            File.WriteAllLines(@"C:\\text_data_dump.txt", allStrings);
        }

        static void DecodeAudioTest()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\music\loadingmusic\wav");
            //var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\effects\interface\hacking\wav");
            //var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\effects\quest\mq13\wav");

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var objects = Decima.CoreBinary.Load(file, false);
                var wave = objects[0] as GameData.WaveResource;

                //var format = new Mp3WaveFormat(wave.SampleRate, wave.ChannelCount, wave.FrameSize, (int)wave.BitsPerSecond);
                //var rs = new RawSourceWaveStream(ms, WaveFormat.CreateCustomFormat(WaveFormatEncoding.MpegLayer3, wave.SampleRate, wave.ChannelCount, (int)(wave.BitsPerSecond / 8), wave.BlockAlignment, wave.BitsPerSample));

                var ms = new System.IO.MemoryStream(wave.WaveData.ToArray());
                RawSourceWaveStream rs = null;

                if (wave.Encoding == GameData.EWaveDataEncoding.MP3)
                    rs = new RawSourceWaveStream(ms, new Mp3WaveFormat(wave.SampleRate, wave.ChannelCount, 0, (int)wave.BitsPerSecond));
                else if (wave.Encoding == GameData.EWaveDataEncoding.PCM)
                    rs = new RawSourceWaveStream(ms, new WaveFormat(wave.SampleRate, 16, wave.ChannelCount));

                if (wave.Encoding == GameData.EWaveDataEncoding.MP3)
                {
                    using (var wo = new WaveOutEvent())
                    {
                        wo.Init(rs);
                        wo.Play();

                        while (wo.PlaybackState == PlaybackState.Playing)
                            System.Threading.Thread.Sleep(50);
                    }
                }
            }
        }
    }
}
