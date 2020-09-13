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
            //DecodeSavesTest();
            //DecodeArchivesTest();
            DecodeReserializeQuickTest();
            //DecodeQuickTest();
            //DecodeAllFilesTest();
            //DecodeLocalizationTest();
            //DecodeAudioTest();
        }

        static void DecodeSavesTest()
        {
            var a = new Decima.SaveGameProfile(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\profile\profile.dat");
            a.ReadProfile();

            var b = new Decima.SaveGameSystem(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\manualsave0\checkpoint.dat");

            var c = new Decima.SaveGameSystem(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\quicksave4\checkpoint.dat");
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

        static void DecodeReserializeQuickTest()
        {
            var files = new string[]
            {
                @"models\weapons\anti_gravity_cannon\model\model.core",
                @"entities\characters\models\humanoid_player.core",
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

                string tempPath = Path.ChangeExtension(fullPath, ".tmp");
                CoreBinary.Save(tempPath, objects);

                byte[] d1 = File.ReadAllBytes(fullPath);
                byte[] d2 = File.ReadAllBytes(tempPath);

                if (d1.Length != d2.Length)
                    throw new Exception("Re-serialized file length doesn't match");

                for (int i = 0; i < d1.Length; i++)
                {
                    if (d1[i] != d2[i])
                        throw new Exception($"File data doesn't match at offset {i:X} in new file");
                }

                File.Delete(tempPath);
            }
        }

        static void DecodeQuickTest()
        {
            var files = new string[]
            {
                @"models\weapons\anti_gravity_cannon\model\model.core",
                @"entities\characters\models\humanoid_player.core",
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
                    if (obj is Decima.HZD.LocalizedTextResource asResource)
                        allStrings.Add(asResource.GetStringForLanguage(Decima.HZD.ELanguage.English));
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
