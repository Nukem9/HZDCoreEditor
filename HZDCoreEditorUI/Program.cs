using Decima;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HZDCoreEditorUI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RTTI.SetGameMode(GameType.HZD);

            if (args.Length > 0)
            {
                Application.Run(new UI.FormCoreView(args[0]));

                return;
            }
            /*
            RTTI.SetGameMode(GameType.DS);
            var objects = new CoreBinary().FromFile(@"C:\Program Files (x86)\Steam\steamapps\common\Death Stranding\data\extracted\ds/models/characters/sam_sam/core/sam_head_def/model/parts/mesh_face_lx.core");

            foreach (var obj in objects)
            {
                if (obj is Decima.DS.ShaderResource sr)
                {
                    foreach (var s in sr.Shader.Programs)
                    {
                        File.WriteAllBytes($"C:\\shdr\\dumpedshader_{CRC32C.Checksum(s.HlslData)}.bin", s.HlslData);

                        var p = new System.Diagnostics.Process();
                        p.StartInfo.FileName = @"C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x64\fxc.exe";
                        p.StartInfo.Arguments = $"-dumpbin C:\\shdr\\dumpedshader_{CRC32C.Checksum(s.HlslData)}.bin";
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.CreateNoWindow = true;
                        p.Start();
                        File.WriteAllText($"C:\\shdr\\dumpedshader_{CRC32C.Checksum(s.HlslData)}.txt", p.StandardOutput.ReadToEnd());
                    }
                }
            }

            Application.Run(new UI.FormCoreView(objects.ToList()));

            var x = new string[]
            {
                    //@"entities/weapons/heavy_weapons/heavy_railgun.core",
                    //@"entities/weapons/bows_and_arrows/precision_bowsuperior.core",
                    //@"entities/weapons/bows_and_arrows/precision_bowmaster.core",
                    //@"entities/weapons/bows_and_arrows/precision_bow.core",
                    //@"entities/characters/humanoids/player/player_components.core",
                    @"entities/characters/humanoids/player/playercharacter.core",
                    @"entities/characters/humanoids/player/costumes/playercostume_advancedwarbot.core",
                    //@"models/characters/humans/aloyundergarment/animation/partsets.core",
                    @"prefetch/fullgame.prefetch.core",
            };

            using (var archive = new PackfileReader(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\Initial.bin"))
            {
                foreach (var path in x)
                    if (archive.ContainsFile(path))
                        archive.ExtractFile(path, Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", path), true);
            }

            using (var archive = new PackfileReader(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\Remainder.bin"))
            {
                foreach (var path in x)
                    if (archive.ContainsFile(path))
                        archive.ExtractFile(path, Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", path), true);
            }

            using (var archive = new PackfileReader(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\DLC1.bin"))
            {
                foreach (var path in x)
                    if (archive.ContainsFile(path))
                        archive.ExtractFile(path, Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", path), true);
            }

            using (var archive = new PackfileReader(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\Patch.bin"))
            {
                foreach (var path in x)
                    if (archive.ContainsFile(path))
                        archive.ExtractFile(path, Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", path), true);
            }

            {
                string file = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\weapons\heavy_weapons\heavy_railgun.core";
                var objects = new CoreBinary().FromFile(file);

                var resource = objects.First(x => x is Decima.HZD.InventoryAmmoEjectorResource) as Decima.HZD.InventoryAmmoEjectorResource;
                resource.AllowsMoving = true;
                resource.AllowMovingWhileOperating = true;
                resource.AllowsCrouching = true;
                resource.AllowsJumping = false;
                resource.AllowsDiveRolling = true;
                resource.AllowsVaulting = true;
                resource.AllowsSliding = true;
                resource.AllowsSprinting = true;
                resource.StowOnExtendedMovement = false;
                resource.CanActivateWhileFallingAndJumping = false;
                resource.CanWieldWhileMounted = true;

                var bulletEjector = objects.First(x => x is Decima.HZD.BulletEjectorResource) as Decima.HZD.BulletEjectorResource;
                bulletEjector.IsAutoHoming = true;
                bulletEjector.HomeInOnHumanoids = true;
                bulletEjector.FireDuration = 0.05f;
                bulletEjector.FireLoopSound.ExternalFile = "sounds/effects/weapons/robot/grey_wolf/greywolf_machinegun.soundbank";
                bulletEjector.FireLoopSound.Type = BaseRef.Types.ExternalCoreUUID;
                bulletEjector.FireLoopSound.GUID = "{CEB37FDD-7687-F043-A0FF-34433D724366}";

                var magResource = objects.First(x => x is Decima.HZD.MagazineResource) as Decima.HZD.MagazineResource;
                magResource.InfiniteAmmo = true;
                magResource.RoundsPerMagazine = 1000;
                magResource.InitialAmmo = 1000;

                objects.ToFile(file);
            }

            {
                string file = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\characters\humanoids\player\player_components.core";
                var objects = new CoreBinary().FromFile(file);

                var bvcr = objects.Where(x => x is Decima.HZD.BodyVariantComponentResource).ElementAt(0) as Decima.HZD.BodyVariantComponentResource;
                //bvcr.ModelPartResource.ExternalFile = "entities/characters/humanoids/player/costumes/playercostume_carjatrader_heavy";
                //bvcr.ModelPartResource.Type = BaseRef<Decima.HZD.ModelPartResource>.Types.ExternalCoreUUID;
                //bvcr.MeshResource.GUID.AssignFromString("99A15806-5C6C-384B-B8A3-8D7E627A7A15");
                //bvcr.Variants[32].ExternalFile = "entities/characters/humanoids/individual_characters/story_characters/elizabeths_hologram";
                //bvcr.Variants[32].GUID.AssignFromString("0EF69237-1F64-424F-9666-B4C3146B2657");

                //bvcr.Variants[32] = bvcr.Variants[38];

                //objects.ToFile(file);
            }

            {
                string file = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\characters\humanoids\player\playercharacter.core";
                var objects = new CoreBinary().FromFile(file);

                var soldierResource = objects.OfType<Decima.HZD.SoldierResource>().Single(x => x.Name.Value.Equals("AloyPlayerCharacter"));
                soldierResource.EntityComponentResources.RemoveAt(90);
                soldierResource.Name = "AloyPlayerCharacter_Nukem";

                objects.ToFile(file);
            }

            {
                string file = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\characters\humanoids\player\costumes\playercostume_advancedwarbot.core";
                var objects = new CoreBinary().FromFile(file);

                var resource = objects.First(x => x is Decima.HZD.HumanoidBodyVariant) as Decima.HZD.HumanoidBodyVariant;

                resource.ModelPartResource.Type = BaseRef.Types.ExternalCoreUUID;
                resource.ModelPartResource.ExternalFile = "entities/characters/humanoids/individual_characters/story_characters/rost";
                resource.ModelPartResource.GUID = "{1C40AE2E-672A-9632-8645-EB9C671CBBE0}";

                resource.AbilityPoseDeformerResource.Type = BaseRef.Types.ExternalCoreUUID;
                resource.AbilityPoseDeformerResource.ExternalFile = resource.ModelPartResource.ExternalFile;
                resource.AbilityPoseDeformerResource.GUID = "{2BC85685-3C4A-9147-8412-9A8BB10EF60A}";

                void addComponent(string guid, string file)
                {
                    resource.EntityComponentResources.Add(new Decima.HZD.Ref<Decima.HZD.EntityComponentResource>()
                    {
                        Type = BaseRef.Types.ExternalCoreUUID,
                        GUID = guid,
                        ExternalFile = file,
                    });
                }

                resource.EntityComponentResources.Clear();
                addComponent("228A3D48-76CE-BB3B-B7F7-214C91049AB8", "models/characters/humans/heads/male/markp/animation/facialanimationcomponent");
                addComponent("C4E1D727-1B0C-FA41-AF2A-FE73E532B76D", "entities/characters/humanoids/individual_characters/story_characters/rost");
                addComponent("D0AC714B-AD50-C642-AB6B-52DA96D89DA7", "entities/characters/components/humanoidragdollcomponents");
                addComponent("414F0F0F-DCAF-F446-8BCC-49CADAE919D7", "entities/characters/humanoids/individual_characters/story_characters/rost");

                resource.ComponentResourceOverrides.Clear();
                resource.ComponentResourceOverrides.Add(new Decima.HZD.Ref<Decima.HZD.EntityComponentResource>()
                {
                    Type = BaseRef.Types.ExternalCoreUUID,
                    GUID = "{568E5C6C-81DF-2B4D-AC34-3816467AD164}",
                    ExternalFile = "entities/characters/humanoids/individual_characters/story_characters/rost",
                });

                objects.ToFile(file);
            }

            {
                string file = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\entities\weapons\bows_and_arrows\precision_bow.core";
                var objects = new CoreBinary().FromFile(file);

                var resource = objects.First(x => x is Decima.HZD.InventoryAmmoEjectorResource) as Decima.HZD.InventoryAmmoEjectorResource;
                resource.AllowsMoving = true;
                resource.AllowMovingWhileOperating = true;
                resource.AllowsCrouching = true;
                resource.AllowsJumping = true;
                resource.AllowsDiveRolling = true;
                resource.AllowsVaulting = true;
                resource.AllowsSliding = true;
                resource.AllowsSprinting = true;
                resource.StowOnExtendedMovement = false;
                resource.CanActivateWhileFallingAndJumping = true;
                resource.CanWieldWhileMounted = true;

                // Overwrite AmmoEjectorWeaponResource
                //resource.EntityComponentResources.Clear();

                //objects.ToFile(file);
            }

            {
                var prefetchCore = new CoreBinary().FromFile(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\prefetch\fullgame.prefetch.core");
                var prefetchList = prefetchCore.First(x => x is Decima.HZD.PrefetchList) as Decima.HZD.PrefetchList;

                foreach (string path in x)
                {
                    if (path.Contains("fullgame.prefetch.core"))
                        continue;

                    RebuildSizesForFile(prefetchList, path.Replace(".core", ""), Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", path));
                    RebuildLinksForFile(prefetchList, path.Replace(".core", ""), Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", path));
                }

                prefetchCore.ToFile(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\prefetch\fullgame.prefetch.core");
            }

            using (var archive = new PackfileWriter(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\Patch_new.bin", false, true))
            {
                archive.BuildFromFileList(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", x);
            }
            
            // Needs to be moved to its own unit test project

            RTTI.SetGameMode(GameType.DS);
            TestsDS.DecodeReserializeQuickCoreFilesTest();
            TestsDS.DecodeReserializeAllCoreFilesTest();
            TestsDS.DecodeQuickCoreFilesTest();
            TestsDS.DecodeAllCoreFilesTest();
            TestsDS.PackArchivesQuickTest();
            TestsDS.PackArchivesTest();
            TestsDS.DecodeArchivesBenchmarkTest();
            TestsDS.DecodeQuickArchivesTest();

            RTTI.SetGameMode(GameType.HZD);
            TestsHZD.DecodeSavesTest();
            TestsHZD.DecodeReserializeQuickCoreFilesTest();
            TestsHZD.DecodeReserializeAllCoreFilesTest();
            TestsHZD.DecodeQuickCoreFilesTest();
            TestsHZD.DecodeAllCoreFilesTest();
            TestsHZD.PackArchivesQuickTest();
            TestsHZD.PackArchivesTest();
            TestsHZD.DecodeQuickArchivesTest();
            TestsHZD.DecodeAllArchivesTest();
            */
            
            //ExtractHZDLocalization();

            Application.Run(new UI.FormCoreView());

            /*
            ExtractHZDLocalization();
            ExtractHZDAudio();
            */
        }

        static void RebuildSizesForFile(Decima.HZD.PrefetchList prefetchList, string fileName, string physicalFilePath)
        {
            int index = prefetchList.Files.IndexOf(prefetchList.Files.Single(x => x.Path.Value.Equals(fileName)));
            prefetchList.Sizes[index] = (int)(new FileInfo(physicalFilePath).Length);
        }

        static void RebuildLinksForFile(Decima.HZD.PrefetchList prefetchList, string fileName, string physicalFilePath)
        {
            RebuildLinksForFile(prefetchList, fileName, CoreBinary.FromFile(physicalFilePath));
        }

        static void RebuildLinksForFile(Decima.HZD.PrefetchList prefetchList, string fileName, CoreBinary coreBinary)
        {
            // Convert the old links to a dictionary
            var oldLinks = new Dictionary<int, int[]>();
            int linkIndex = 0;

            for (int i = 0; i < prefetchList.Files.Count; i++)
            {
                int count = prefetchList.Links[linkIndex];

                var indices = prefetchList.Links
                    .Skip(linkIndex + 1)
                    .Take(count)
                    .ToArray();

                oldLinks.Add(i, indices);
                linkIndex += count + 1;
            }

            // Regenerate links for this specific file (don't forget to remove duplicates (Distinct()!!!))
            int getFileIndex(string path)
            {
                return prefetchList.Files.IndexOf(prefetchList.Files.Single(x => x.Path.Value.Equals(path)));
            }

            oldLinks[getFileIndex(fileName)] = coreBinary.GetAllReferences()
                .Where(x => x.Type == BaseRef.Types.ExternalCoreUUID)
                .Select(x => getFileIndex(x.ExternalFile.Value))
                .Distinct()
                .ToArray();

            // Dictionary of links -> linear array
            prefetchList.Links.Clear();

            for (int i = 0; i < prefetchList.Files.Count; i++)
            {
                var indices = oldLinks[i];
                prefetchList.Links.Add(indices.Count());

                foreach (int index in indices)
                    prefetchList.Links.Add(index);
            }
        }

        static void ExtractReferences()
        {
            var prefetchCore = CoreBinary.FromFile(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\prefetch\fullgame.prefetch.core");
            var prefetchList = prefetchCore.First(x => x is Decima.HZD.PrefetchList) as Decima.HZD.PrefetchList;

            var files = prefetchList.Files;
            var fileRefMap = new HashSet<int>[files.Count];

            // Keep a lookup table since linear searches are incredibly slow
            var fileIndexLUT = new Dictionary<string, int>();

            for (int i = 0; i < files.Count; i++)
                fileIndexLUT[files[i].Path] = i;

            // Gather every ExternalCoreUUID reference from every Core file and compile it into a list
            Parallel.ForEach(prefetchList.Files, (file, _, index) =>
            {
                fileRefMap[index] = new HashSet<int>();

                try
                {
                    var core = CoreBinary.FromFile(Path.Combine(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\", $"{file.Path}.core"), true);

                    foreach (var baseRef in core.GetAllReferences())
                    {
                        if (baseRef.Type == BaseRef.Types.ExternalCoreUUID)
                            fileRefMap[index].Add(fileIndexLUT[baseRef.ExternalFile.Value]);
                    }
                }
                catch (Exception)
                {
                }
            });

            // Rebuild the index array - a counter (N) followed by N indices
            var indices = new List<int>();

            for (int i = 0; i < files.Count; i++)
            {
                indices.Add(fileRefMap[i].Count);

                foreach (int fileIndex in fileRefMap[i])
                    indices.Add(fileIndex);
            }
        }

        static void ExtractHZDLocalization()
        {
            var files = Directory.GetFiles(@"E:\hzd\localized\", "*.core", SearchOption.AllDirectories);
            var sb = new StringBuilder();

            foreach (string file in files)
            {
                Console.WriteLine(file);
                bool first = true;

                var core = CoreBinary.FromFile(file);

                foreach (var obj in core)
                {
                    if (obj is Decima.HZD.LocalizedTextResource asResource)
                    {
                        if (first)
                        {
                            sb.AppendLine();
                            sb.AppendLine(file);
                            first = false;
                        }

                        sb.AppendLine(asResource.ObjectUUID + asResource.GetStringForLanguage(Decima.HZD.ELanguage.English));
                    }
                }
            }

            File.WriteAllText(@"E:\hzd\text_data_dump.txt", sb.ToString());
        }

        static void ExtractHZDAudio()
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12\extracted\sounds\", "*.core", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                Console.WriteLine(file);

                var core = CoreBinary.FromFile(file);

                foreach (var obj in core)
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
