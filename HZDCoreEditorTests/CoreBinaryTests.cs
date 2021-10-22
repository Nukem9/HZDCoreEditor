using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HZDCoreEditorTests
{
    [TestClass]
    public class CoreBinaryTests
    {
        private static readonly string GameDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12";
        private static readonly string GameDataPathExtracted = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted";

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

        [TestCategory("GameBasic")]
        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "Intentional for debug")]
        public void DecodeCoreFilesTestTrivial()
        {
            foreach (string file in QuickTestFiles)
            {
                var fullPath = Path.Combine(GameDataPathExtracted, file);

                var core = Decima.CoreBinary.FromFile(fullPath);
            }
        }

        [TestCategory("GameSlow")]
        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "Intentional for debug")]
        public void DecodeCoreFilesTestAll()
        {
            var files = Directory.GetFiles(GameDataPathExtracted, "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                var core = Decima.CoreBinary.FromFile(file);
            }
        }

        [TestCategory("GameSlow")]
        [TestMethod]
        public void DecodeReserializeCoreFilesTestTrivial()
        {
            foreach (string file in QuickTestFiles)
            {
                var fullPath = Path.Combine(GameDataPathExtracted, file);
                var tempPath = Path.ChangeExtension(fullPath, ".tmp");

                var core = Decima.CoreBinary.FromFile(fullPath);
                core.ToFile(tempPath, FileMode.Create);

                var d1 = File.ReadAllBytes(fullPath);
                var d2 = File.ReadAllBytes(tempPath);

                Assert.IsTrue(d1.Length == d2.Length, $"Re-serialized binary file length doesn't match {file} ({d1.Length} != {d2.Length})");

                for (int i = 0; i < d1.Length; i++)
                {
                    Assert.IsTrue(d1[i] == d2[i], $"File data doesn't match at offset {i:X} in {file}");
                }

                File.Delete(tempPath);
            }
        }

        [TestCategory("GameSlow")]
        [TestMethod]
        public void DecodeReserializeCoreFilesTestAll()
        {
            var files = Directory.GetFiles(GameDataPathExtracted, "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                var tempPath = Path.ChangeExtension(file, ".tmp");

                var core = Decima.CoreBinary.FromFile(file);
                core.ToFile(tempPath, FileMode.Create);

                var d1 = File.ReadAllBytes(file);
                var d2 = File.ReadAllBytes(tempPath);

                Assert.IsTrue(d1.Length == d2.Length, $"Re-serialized binary file length doesn't match {file} ({d1.Length} != {d2.Length})");

                for (int i = 0; i < d1.Length; i++)
                {
                    Assert.IsTrue(d1[i] == d2[i], $"File data doesn't match at offset {i:X} in {file}");
                }

                File.Delete(tempPath);
            }
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void DecodeCoreFileInvalidType()
        {
            var tempCorePath = Path.Combine(Path.GetTempPath(), $"{nameof(DecodeCoreFileInvalidType)}_test.core");

            // Write out a valid header with an invalid type
            using (var bw = new BinaryWriter(File.Open(tempCorePath, FileMode.Create)))
            {
                bw.Write((ulong)0xDEADBEEFDEADC0DE);
                bw.Write((uint)10);

                for (byte i = 0; i < 10; i++)
                    bw.Write(i);
            }

            // Invalid types should throw
            Assert.ThrowsException<InvalidDataException>(() => _ = Decima.CoreBinary.FromFile(tempCorePath, false));

            // Otherwise ignore them
            _ = Decima.CoreBinary.FromFile(tempCorePath);
        }
    }
}
