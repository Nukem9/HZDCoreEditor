using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Decima;

namespace HZDCoreEditorTests
{
    [TestClass]
    public class PackfileTests
    {
        private static readonly string GameDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12";
        private static readonly string GameDataPathExtracted = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted";
        private static readonly string GameRootArchive = "Initial.bin";

        private static readonly string[] QuickTestArchives = new string[]
        {
            "DLC1.bin",
            "DLC1_English.bin",
            "Initial.bin",
            "Initial_English.bin",
            "Remainder.bin",
            "Remainder_English.bin",
        };

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestValidateSingleBin()
        {
            using var archive = new PackfileReader(Path.Combine(GameDataPath, GameRootArchive));
            archive.Validate();
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestValidateMultipleBins()
        {
            foreach (var archiveName in QuickTestArchives)
            {
                using var archive = new PackfileReader(Path.Combine(GameDataPath, archiveName));
                archive.Validate();
            }
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestFileExists()
        {
            using var archive = new PackfileReader(Path.Combine(GameDataPath, GameRootArchive));

            Assert.IsTrue(archive.ContainsFile("prefetch/fullgame.prefetch.core"));
            Assert.IsTrue(archive.ContainsFile(Packfile.SanitizePath("prefetch/fullgame.prefetch")));
            Assert.IsTrue(archive.ContainsFile(Packfile.SanitizePath("prefetch\\fullgame.prefetch.core")));
            //Assert.IsTrue(archive.ContainsFile("models/weapons/heavy_machinegun/model/model.core.stream")); - need to find a file present in Initial.bin
            //Assert.IsTrue(archive.ContainsFile("sounds/effects/world/weather/habitats/fields/weather_fields.soundbank", ".core.stream"));

            Assert.IsFalse(archive.ContainsFile("PREFETCH/fullgame.prefetch.core"));
            Assert.IsFalse(archive.ContainsFile("prefetch\\FULLGAME.prefetch.core"));
            Assert.IsFalse(archive.ContainsFile("some/made/up/path/here.core"));
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestFileExistsByPathId()
        {
            using var archive = new PackfileReader(Path.Combine(GameDataPath, GameRootArchive));

            Assert.IsTrue(Packfile.GetHashForPath(Packfile.SanitizePath("prefetch/fullgame.prefetch")) == 0x2FFF5AF65CD64C0A);
            Assert.IsTrue(archive.ContainsFile(0x2FFF5AF65CD64C0A));

            Assert.ThrowsException<FileNotFoundException>(() => archive.ExtractFile(0xDEADC0DEDEADBEEF, new MemoryStream()));
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestExtractSingleFile()
        {
            using var archive = new PackfileReader(Path.Combine(GameDataPath, GameRootArchive));
            var tempPath = Path.Combine(Path.GetTempPath(), $"{nameof(TestExtractSingleFile)}_extracted.core");

            if (File.Exists(tempPath))
                File.Delete(tempPath);

            archive.ExtractFile("prefetch/fullgame.prefetch.core", tempPath);
            Assert.IsTrue(File.Exists(tempPath));

            File.Delete(tempPath);
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestExtractSingleFileToStream()
        {
            using var archive = new PackfileReader(Path.Combine(GameDataPath, GameRootArchive));
            var testStream = new MemoryStream();

            archive.ExtractFile("prefetch/fullgame.prefetch.core", testStream);

            Assert.IsTrue(testStream.Length > 0);
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestPackAndUnpackTrivial()
        {
            // Generate a couple useless files to pack
            var tempPath = Path.GetTempPath();
            var tempFiles = new List<(string Path, string Text, string CoreName)>();

            for (int i = 0; i < 15; i++)
            {
                var coreName = $"{nameof(TestPackAndUnpackTrivial)}_input_file{i}.core";
                var path = Path.Combine(tempPath, coreName);
                var text = $"{i} Here's my testing file with some data! {i}\r\n";

                File.WriteAllText(path, text);
                tempFiles.Add((path, text, coreName));
            }

            // Write out compressed bin
            var packedArchivePath = Path.Combine(tempPath, $"{nameof(TestPackAndUnpackTrivial)}_packed_archive.bin");

            using (var writeArchive = new PackfileWriter(packedArchivePath, false, FileMode.Create))
                writeArchive.BuildFromFileList(tempPath, tempFiles.Select(x => x.CoreName));

            // Open it back up and validate its contents
            using (var readArchive = new PackfileReader(packedArchivePath))
            {
                readArchive.Validate();

                foreach (var file in tempFiles)
                {
                    Assert.IsTrue(readArchive.ContainsFile(file.CoreName));

                    readArchive.ExtractFile(file.CoreName, file.Path, FileMode.Create);

                    Assert.IsTrue(File.ReadAllText(file.Path).Equals(file.Text));

                    File.Delete(file.Path);
                }
            }

            File.Delete(packedArchivePath);
        }

        [TestCategory("GameSlow")]
        [TestMethod]
        public void TestPackAndUnpack()
        {
            // Gather 500 random files to throw into a bin
            var archivePath = Path.Combine(Path.GetTempPath(), $"{nameof(TestPackAndUnpack)}_packed_archive.bin");
            var targetDir = GameDataPathExtracted;

            if (!targetDir.EndsWith('\\'))
                targetDir += "\\";

            var filesToCombine = Directory
                .EnumerateFiles(targetDir, "*.core", SearchOption.AllDirectories)
                .Take(500)
                .Select(f => f.Substring(targetDir.Length))
                .ToArray();

            using (var writeArchive = new PackfileWriter(archivePath, false, FileMode.Create))
                writeArchive.BuildFromFileList(targetDir, filesToCombine);

            // Re-extract all of the contained files into memory
            using (var readArchive = new PackfileReader(archivePath))
            {
                readArchive.Validate();

                using var tempMS = new MemoryStream();

                foreach (string file in filesToCombine)
                {
                    tempMS.Position = 0;
                    tempMS.SetLength(0);
                    readArchive.ExtractFile(file, tempMS);

                    var tempFilePath = Path.Combine(targetDir, file);
                    Assert.IsTrue(tempMS.Length == new FileInfo(tempFilePath).Length);
                }
            }

            File.Delete(archivePath);
        }
    }
}
