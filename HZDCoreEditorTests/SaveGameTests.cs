namespace HZDCoreEditorTests
{
    using Decima;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SaveGameTests
    {
        [Ignore]
        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestReadManualSaveFile()
        {
            var s = new SaveGameSystem(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\manualsave0\checkpoint.dat");
        }

        [Ignore]
        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestReadQuickSaveFile()
        {
            var s = new SaveGameSystem(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\quicksave4\checkpoint.dat");
        }

        [Ignore]
        [TestCategory("GameBasic")]
        [TestMethod]
        public void TestReadSaveProfile()
        {
            var s = new SaveGameProfile(@"C:\Users\Administrator\Documents\Horizon Zero Dawn\Saved Game\profile\profile.dat");

            s.ReadProfile();
        }
    }
}
