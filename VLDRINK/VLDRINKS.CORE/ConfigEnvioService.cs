using Newtonsoft.Json;
using System;
using System.IO;

namespace VLDRINKS.CORE
{
    public class ConfigEnvio
    {
        public decimal CostoEnvio { get; set; }
    }

    public static class ConfigEnvioService
    {
        private static string GetConfigPath()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string path = Path.Combine(basePath, "App_Data", "config_envio.json");

            return path;
        }

        public static ConfigEnvio Leer()
        {
            string path = GetConfigPath();

            if (!File.Exists(path))
            {
                var defaultConfig = new ConfigEnvio { CostoEnvio = 0 };
                Guardar(defaultConfig);
                return defaultConfig;
            }

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ConfigEnvio>(json);
        }

        public static void Guardar(ConfigEnvio config)
        {
            string path = GetConfigPath();

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
