using Decima;
using System;
using System.Diagnostics;
using System.IO;

namespace HZDCoreEditor
{
    public static class TestsDS
    {
        private static readonly string GameDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Death Stranding\data";
        private static readonly string GameDataPathExtracted = @"C:\Program Files (x86)\Steam\steamapps\common\Death Stranding\data\extracted";

        private static readonly string[] QuickTestArchives = new string[]
        {
            "7017f9bb9d52fc1c4433599203cc51b1.bin",
        };

        private static readonly string[] QuickTestFiles = new string[]
        {
            @"interface\menu\ds\ui_menu_hold_legend_prefab.core",
            @"ds\radio\radio_sequence\global\rd_menu_special_new_item.core",
            @"ds\location\area01\pre_constructions\area01_pre_constructions.core",
            @"ds\sounds\wwise_bnk_collections\sd_wwise_bnk_collection_game_resident.core",
            @"ds\textures\detail_textures\dsdamagetexturearray.core",
            @"localized\sentences\ds\loc_object_names.core",
            @"ds\generic_demo\sqn_common_sq_cs99_s00500.core",
            @"ds\generic_demo\sqn_common_sq_cs99_s00400.core",
            @"game\ds_online_sync_config.core",
            @"ambience\newambiencemanager.core",
            @"ds\models\characters\sam_sam\core\sam_body_black\model\parts\mesh_bodynaked_lx.core",
            @"ds\models\characters\sam_sam\core\sam_body_black\model\parts\mesh_upperbody_lx.core",
        };

        public static void DecodeReserializeQuickCoreFilesTest()
        {
            foreach (string file in QuickTestFiles)
            {
                string fullPath = Path.Combine(GameDataPathExtracted, file);
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

        public static void DecodeReserializeAllCoreFilesTest()
        {
            var files = Directory.GetFiles(GameDataPathExtracted, "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var objects = CoreBinary.Load(file, true);

                string tempPath = Path.ChangeExtension(file, ".tmp");
                CoreBinary.Save(tempPath, objects);

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

                var objects = CoreBinary.Load(fullPath);
            }
        }

        public static void DecodeAllCoreFilesTest()
        {
            var files = Directory.GetFiles(GameDataPathExtracted, "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var objects = CoreBinary.Load(file, true);
            }
        }

        public static void DecodeArchivesBenchmarkTest()
        {
            string basePath = GameDataPath;

            string prefetchPath = Path.Combine(basePath, "extracted", "prefetch/fullgame.prefetch.core");
            Directory.CreateDirectory(Path.GetDirectoryName(prefetchPath));

            var archive = new Packfile(Path.Combine(basePath, "7017f9bb9d52fc1c4433599203cc51b1.bin"));
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 500; i++)
                archive.ExtractFile("prefetch/fullgame.prefetch.core", prefetchPath, true);

            sw.Stop();
            Debugger.Log(0, "Info", $"Repeated archive extraction took {sw.ElapsedMilliseconds} milliseconds");
        }

        public static void DecodeQuickArchivesTest()
        {
            string basePath = GameDataPath;

            foreach (var file in QuickTestArchives)
            {
                var archive = new Packfile(Path.Combine(basePath, file));

                // Prefetch has to be extracted first in order to get any kind of file names
                string prefetchPath = Path.Combine(basePath, "extracted", "prefetch/fullgame.prefetch.core");
                Directory.CreateDirectory(Path.GetDirectoryName(prefetchPath));

                archive.ExtractFile("prefetch/fullgame.prefetch.core", prefetchPath, true);
                var prefetchObject = CoreBinary.Load(prefetchPath)[0] as Decima.DS.PrefetchList;

                // Do the /actual/ extraction
                foreach (var assetPath in prefetchObject.Files)
                {
                    string localCorePath = assetPath.Path + ".core";

                    if (!archive.ContainsFile(localCorePath))
                        continue;

                    var physicalPath = Path.Combine(basePath, "extracted", localCorePath);
                    var physicalDir = Path.GetDirectoryName(physicalPath);

                    Directory.CreateDirectory(physicalDir);
                    archive.ExtractFile(localCorePath, physicalPath, true);
                }
            }
        }
    }
}
