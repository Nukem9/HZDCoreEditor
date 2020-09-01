using Decima;
using Decima.HZD;
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
            //DecodeQuickTest();
            //DecodeAllFilesTest();
            //DecodeLocalizationTest();
            //DecodeAudioTest();
            DecodeArchivesTest();
        }

        static void DecodeArchivesTest()
        {
            string basePath = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\";

            var archiveList = new string[]
            {
                "DLC1.bin",
                "DLC1_English.bin",
                "Initial.bin",
                "Initial_English.bin",
                "Remainder.bin",
                "Remainder_English.bin",
            };

            foreach (var file in archiveList)
            {
                var indexFile = new PackfileIndex(Path.Combine(basePath, Path.ChangeExtension(file, ".idx")));
                var archive = new Packfile(Path.Combine(basePath, file));

                foreach (var entry in archive.FileEntries)
                {
                    if (!indexFile.ResolvePathByHash(entry.PathHash, out string fn))
                        throw new Exception();

                    var physicalPath = Path.Combine(basePath, "extracted", fn);
                    var physicalDir = Path.GetDirectoryName(physicalPath);

                    Directory.CreateDirectory(physicalDir);
                    archive.ExtractFile(fn, physicalPath);
                }

                Console.WriteLine(file);
            }
        }

        static void DecodeQuickTest()
        {
            var files = new string[]
            {
                @"models\characters\humans\heads\baby\babyaloy\animation\parts\head_lx.core",
                @"models\characters\humans\hair\aloy\model\model.core",
                @"models\animation_managers\characters\animals\blackrat\blackrat_blackrat.core",
                @"sounds\music\world\world.core",
                @"shaders\ecotope\texturesetarrays\terrain_texture_array.core",
                @"animation_objects\ledge_climb\network\moaf_honst_fight_intro_network.core",
                @"movies\movielist.core",
                @"entities\trackedgamestats.core",
                @"localized\sentences\aigenerated\nora_adult_male1_1\sentences.core",
                @"interface\textures\markers\ui_marker_compass_bunker.core",
                @"levels\worlds\world\tiles\tile_x05_y-01\layers\lighting\cubezones\cubemapzone_07_cube.core",
                @"levels\worlds\world\tiles\tile_x05_y-01\layers\lighting\cubezones\cubezones_foundry_1.core",
                @"textures\lighting_setups\skybox\star_field.core",
                @"textures\base_maps\clouds_512.core",
                @"textures\detail_textures\buildingblock\buildingblock_detailmap_array.core",
                @"sounds\effects\dlc1\weapons\firebreather\wav\fire_loop_flames_01_m.core",
                @"models\building_blocks\nora\dressing\components\dressing_b125_c006_resource.core",
                @"entities\characters\models\humanoid_player.core",
                @"telemetry\designtelemetry.core",
                @"worlddata\worlddatapacking.core",
                @"entities\weapons\heavy_weapons\heavy_railgun_cables.core",
                @"entities\collectables\collectable_datapoint\datapoint_world.core",
                @"entities\shops\shops.core",
                @"levels\quests.core",
                @"levels\game.core",
                @"levels\worlds\world\leveldata\terrain.core",
                @"weather\defaultweathersystem.core",
                @"climates\regions\faro\faro_master_climate.core",
                @"entities\armor\outfits\outfits.core",
                @"models\weapons\raptordisc_playerversion\model\model.core",
                @"loadouts\loadout.core",
                @"levels\worlds\world\quests\mainquests\mq15_themountainthatfell_files\mq15_quest.core",
                @"entities\characters\models\humanoid_civilian.core",
                @"system\waves\white_noise_0dbfs.core",
            };

            foreach (string file in files)
            {
                string fullPath = Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", file);
                Console.WriteLine(fullPath);

                var objects = CoreBinary.Load(fullPath);
            }
        }

        static void DecodeAllFilesTest()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var objects = CoreBinary.Load(file, true);
            }
        }

        static void DecodeLocalizationTest()
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
                    if (obj is LocalizedTextResource asResource)
                        allStrings.Add(asResource.GetStringForLanguage(ELanguage.English));
                }
            }

            File.WriteAllLines(@"C:\text_data_dump.txt", allStrings);
        }

        static void DecodeAudioTest()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\", "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var coreObjects = CoreBinary.Load(file, true);

                foreach (var obj in coreObjects)
                {
                    var wave = obj as WaveResource;

                    if (wave == null)
                        continue;

                    if (wave.IsStreaming)
                        continue;

                    //var data = File.ReadAllBytes(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\music\loadingmusic\wav\temp.core");

                    using (var ms = new System.IO.MemoryStream(wave.WaveData.ToArray()))
                    //using (var ms = new MemoryStream(data))
                    {
                        RawSourceWaveStream rs = null;

                        if (wave.Encoding == EWaveDataEncoding.MP3)
                            rs = new RawSourceWaveStream(ms, new Mp3WaveFormat(wave.SampleRate, wave.ChannelCount, wave.FrameSize, (int)wave.BitsPerSecond));
                        else if (wave.Encoding == EWaveDataEncoding.PCM)
                            rs = new RawSourceWaveStream(ms, new WaveFormat(wave.SampleRate, 16, wave.ChannelCount));

                        string outFile = Path.Combine(Path.GetDirectoryName(file), wave.Name.Value + ".wav");

                        if (rs != null)
                            WaveFileWriter.CreateWaveFile(outFile, rs);
                    }
                }
            }
        }
    }
}
