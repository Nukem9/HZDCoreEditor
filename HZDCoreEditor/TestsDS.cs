﻿using Decima;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            @"ds\game\ds_online_sync_config.core",
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

                var core = new CoreBinary().FromFile(file);
            }
        }

        public static void PackArchivesQuickTest()
        {
            string archivePath = Path.Combine(GameDataPath, "test_packed_archive.tmp");

            using (var testArchive = new PackfileWriter(archivePath, true, true))
            {
                testArchive.BuildFromFileList(GameDataPathExtracted, QuickTestFiles);
            }

            using (var testArchive = new PackfileReader(archivePath))
            {
                testArchive.Validate();

                // Re-extract all of the contained files
                foreach (string file in QuickTestFiles)
                {
                    string tempFilePath = Path.Combine(GameDataPathExtracted, $"{file}.tmp");

                    testArchive.ExtractFile(file, tempFilePath, true);
                    File.Delete(tempFilePath);
                }
            }

            File.Delete(archivePath);
        }

        public static void PackArchivesTest()
        {
            string archivePath = Path.Combine(GameDataPath, "test_packed_archive.tmp");
            string targetDir = GameDataPathExtracted;

            if (!targetDir.EndsWith('\\'))
                targetDir += "\\";

            var filesToCombine = Directory
                .EnumerateFiles(targetDir, "*.core", SearchOption.AllDirectories)
                .Take(500)
                .Select(f => f.Substring(targetDir.Length))
                .ToArray();

            using (var testArchive = new PackfileWriter(archivePath, true, true))
            {
                testArchive.BuildFromFileList(targetDir, filesToCombine);
            }

            using (var testArchive = new PackfileReader(archivePath))
            {
                testArchive.Validate();

                // Re-extract all of the contained files
                foreach (string file in filesToCombine)
                {
                    string tempFilePath = Path.Combine(targetDir, $"{file}.tmp");

                    testArchive.ExtractFile(file, tempFilePath, true);
                    File.Delete(tempFilePath);
                }
            }

            File.Delete(archivePath);
        }

        public static void DecodeArchivesBenchmarkTest()
        {
            string basePath = GameDataPath;

            string prefetchPath = Path.Combine(basePath, "extracted", "prefetch/fullgame.prefetch.core");
            Directory.CreateDirectory(Path.GetDirectoryName(prefetchPath));

            var archive = new PackfileReader(Path.Combine(basePath, "7017f9bb9d52fc1c4433599203cc51b1.bin"));
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
                var archive = new PackfileReader(Path.Combine(basePath, file));

                // Prefetch has to be extracted first in order to get any kind of file names
                string prefetchPath = Path.Combine(basePath, "extracted", "prefetch/fullgame.prefetch.core");
                Directory.CreateDirectory(Path.GetDirectoryName(prefetchPath));

                archive.ExtractFile("prefetch/fullgame.prefetch.core", prefetchPath, true);
                var prefetchObject = new CoreBinary().FromFile(prefetchPath).First() as Decima.DS.PrefetchList;

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
