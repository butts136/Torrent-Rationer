using Newtonsoft.Json;
using TorrentRationer.Models;

namespace TorrentRationer.Services
{
    public class ConfigurationService
    {
        private readonly string _configPath;
        private AppConfiguration _config;

        public ConfigurationService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var configDir = Path.Combine(appDataPath, "TorrentRationer");
            Directory.CreateDirectory(configDir);
            _configPath = Path.Combine(configDir, "config.json");
            _config = LoadConfiguration();
        }

        public AppConfiguration GetConfiguration()
        {
            return _config;
        }

        public void SaveConfiguration(AppConfiguration config)
        {
            _config = config;
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }

        private AppConfiguration LoadConfiguration()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    var json = File.ReadAllText(_configPath);
                    return JsonConvert.DeserializeObject<AppConfiguration>(json) ?? new AppConfiguration();
                }
                catch
                {
                    return new AppConfiguration();
                }
            }
            return new AppConfiguration();
        }

        public string GetConfigPath() => _configPath;
    }
}
