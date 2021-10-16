using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace HZDCoreSearch
{
    public static class SettingsManager
    {
        private const string SettingsFile = "settings.json";

        public static Settings Settings { get; private set; }
        private static readonly object _lock = new object();

        public static void Load()
        {
            try
            {
                lock (_lock)
                {
                    var json = File.ReadAllText(SettingsFile); 
                    Settings = JsonConvert.DeserializeObject<Settings>(json);
                }
                return;
            }
            catch { }

            Settings = new Settings();
        }

        public static async Task Save()
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    var json = JsonConvert.SerializeObject(Settings);
                    File.WriteAllText(SettingsFile, json);
                }
            });
        }
    }
    
    public class Settings
    {
        public string SearchFolderDir { get; set; }
        public string SearchFolderText { get; set; }
    }
}
