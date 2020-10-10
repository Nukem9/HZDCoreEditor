using Decima;
using System;
using System.IO;
using System.Linq;

namespace HZDCoreEditor
{
    public static class TestsHZD
    {
        private static readonly string GameDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12";
        private static readonly string GameDataPathExtracted = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\";

        private static readonly string[] QuickTestArchives = new string[]
        {
            "DLC1.bin",
            "DLC1_English.bin",
            "Initial.bin",
            "Initial_English.bin",
            "Remainder.bin",
            "Remainder_English.bin",
        };

        private static readonly string[] QuickTestFiles = new string[]
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

        public static void DecodeSavesTest()
        {
            var a = new Decima.SaveGameProfile(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\profile\profile.dat");
            var b = new Decima.SaveGameSystem(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\manualsave0\checkpoint.dat");
            var c = new Decima.SaveGameSystem(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\quicksave4\checkpoint.dat");

            a.ReadProfile();

            /*
            var objects = new List<object>();
            objects.Add(a.Profile);
            objects.Add(b.GlobalGameModule);
            objects.Add(b.GlobalStreamingStrategyManagerGame);
            objects.Add(b.GlobalSceneManagerGame);
            objects.Add(b.GlobalFactDatabase);
            objects.Add(b.GlobalGameSettings);
            objects.Add(b.GlobalWorldState);
            objects.Add(b.GlobalMapZoneManager);
            objects.Add(b.GlobalPickUpDatabaseGame);
            objects.Add(b.GlobalQuestSystem);
            objects.Add(b.GlobalCountdownTimerManager);
            objects.Add(b.GlobalWorldEncounterManager);
            objects.Add(b.GlobalEntityManagerGame);
            objects.Add(b.GlobalMenuBadgeManager);
            objects.Add(b.GlobalCollectableManager);
            objects.Add(b.GlobalPlayerGame);
            objects.Add(b.GlobalLocationMarkerManager);
            objects.Add(b.GlobalExplorationSystem);
            objects.Add(b.GlobalBuddyManager);
            objects.Add(b.GlobalWeatherSystem);
            Application.Run(new UI.FormCoreView(objects));
            */
        }

        public static void DecodeReserializeQuickCoreFilesTest()
        {
            foreach (string file in QuickTestFiles)
            {
                string fullPath = Path.Combine(GameDataPathExtracted, file);
                Console.WriteLine(fullPath);

                var core = new CoreBinary().FromFile(fullPath);

                string tempPath = Path.ChangeExtension(fullPath, ".tmp");
                core.ToFile(tempPath);

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

        public static void DecodeReserializeAllCoreFilesTest()
        {
            var files = Directory.GetFiles(GameDataPathExtracted, "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var core = new CoreBinary().FromFile(file, true);

                string tempPath = Path.ChangeExtension(file, ".tmp");
                core.ToFile(tempPath);

                byte[] d1 = File.ReadAllBytes(file);
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

        public static void DecodeQuickCoreFilesTest()
        {
            foreach (string file in QuickTestFiles)
            {
                string fullPath = Path.Combine(GameDataPathExtracted, file);
                Console.WriteLine(fullPath);

                var core = new CoreBinary().FromFile(fullPath);
            }
        }

        public static void DecodeAllCoreFilesTest()
        {
            var files = Directory.GetFiles(GameDataPathExtracted, "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var core = new CoreBinary().FromFile(file, true);
            }
        }

        public static void PackArchivesQuickTest()
        {
            string archivePath = Path.Combine(GameDataPath, "test_packed_archive.tmp");

            using (var testArchive = new Packfile(archivePath, FileMode.Create))
            {
                testArchive.BuildFromFileList(GameDataPathExtracted, QuickTestFiles);
            }

            using (var testArchive = new Packfile(archivePath, FileMode.Open))
            {
                testArchive.Validate();
            }

            File.Delete(archivePath);
        }

        public static void PackArchivesTest()
        {
            string archivePath = Path.Combine(GameDataPath, "test_packed_archive.tmp");

            using (var testArchive = new Packfile(archivePath, FileMode.Create))
            {
                string targetDir = GameDataPathExtracted;

                if (!targetDir.EndsWith('\\'))
                    targetDir += "\\";

                var filesToCombine = Directory
                    .EnumerateFiles(targetDir, "*.core", SearchOption.AllDirectories)
                    .Take(500)
                    .Select(f => f.Substring(targetDir.Length))
                    .ToArray();

                testArchive.BuildFromFileList(targetDir, filesToCombine);
            }

            using (var testArchive = new Packfile(archivePath, FileMode.Open))
            {
                testArchive.Validate();
            }

            File.Delete(archivePath);
        }

        public static void DecodeQuickArchivesTest()
        {
            string basePath = GameDataPath;

            foreach (var file in QuickTestArchives)
            {
                Console.WriteLine(file);

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
            }
        }

        public static void DecodeAllArchivesTest()
        {
            string basePath = GameDataPath;
            var files = Directory.GetFiles(basePath, "*.bin", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var indexFile = new PackfileIndex(Path.ChangeExtension(file, ".idx"));
                var archive = new Packfile(file);

                foreach (var entry in archive.FileEntries)
                {
                    if (!indexFile.ResolvePathByHash(entry.PathHash, out string fn))
                        throw new Exception();

                    var physicalPath = Path.Combine(basePath, "extracted", fn);
                    var physicalDir = Path.GetDirectoryName(physicalPath);

                    Directory.CreateDirectory(physicalDir);
                    archive.ExtractFile(fn, physicalPath);
                }
            }
        }
    }
}
