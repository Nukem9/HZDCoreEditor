using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace HZDCoreEditorTests
{
    [TestClass]
    public class PackfileDeviceTests
    {
        private static readonly string GameDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Horizon Zero Dawn\Packed_DX12";

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestDefaultMount()
        {
            using var device = new Decima.PackfileDevice();
            device.MountDefaultArchives(GameDataPath);

            var order = device.ActiveArchives.ToArray();
            Assert.IsTrue(order.Length == 39);
            Assert.IsTrue(order[0].Equals("dlc1.bin"));
            Assert.IsTrue(order[38].Equals("patch.bin"));
        }

        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestMountUnmount()
        {
            using var device = new Decima.PackfileDevice();

            Assert.IsTrue(device.Mount(Path.Combine(GameDataPath, "Patch.bin")));
            Assert.IsTrue(device.Mount(Path.Combine(GameDataPath, "initial.bin")));
            Assert.IsTrue(device.Mount(Path.Combine(GameDataPath, "dlc1.bin")));
            Assert.IsTrue(device.Mount(Path.Combine(GameDataPath, "Remainder_English.bin")));
            Assert.IsFalse(device.Mount(Path.Combine(GameDataPath, "patch_debug.bin")));
            Assert.ThrowsException<ArgumentException>(() => device.Mount(Path.Combine(GameDataPath, "Remainder_English.bin")), "Should fail to mount duplicates");

            var order = device.ActiveArchives.ToArray();
            Assert.IsTrue(order.Length == 4);
            Assert.IsTrue(order[0].Equals("dlc1.bin"));
            Assert.IsTrue(order[1].Equals("initial.bin"));
            Assert.IsTrue(order[2].Equals("remainder_english.bin"));
            Assert.IsTrue(order[3].Equals("patch.bin"));

            device.Unmount("INITIAL.BIN");
            device.Unmount("patch.bin");
            Assert.ThrowsException<ArgumentException>(() => device.Unmount("patch.bin"), "Unmounting file that's not loaded");
            device.Unmount("dlc1.bin");

            order = device.ActiveArchives.ToArray();
            Assert.IsTrue(order.Length == 1);
            Assert.IsTrue(order[0].Equals("remainder_english.bin"));
        }
    }
}
