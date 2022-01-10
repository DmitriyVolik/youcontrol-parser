using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using selenium_test.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_test.Core.Helpers
{
    public static class FileHelper
    {
        static string ConfigurationPath { get; set; } = "config.json";

        public static void SaveConfiguration(Config config)
        {
            File.WriteAllTextAsync(ConfigurationPath, JsonHelper<Config>.ObjectToJson(config));
        }
        public static Config LoadConfiguration()
        {
            if (!File.Exists(ConfigurationPath))
            {
                var config = new Config();
                SaveConfiguration(config);
                return config;
            }
            return JsonHelper<Config>.JsonToObject(File.ReadAllText(ConfigurationPath));
        }
        public static List<string> ReadUrls(Config config)
        {
            try
            {
                return File.ReadAllLines(config.FilePathInput).ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Файл, содержащий ссылки на города отсутствует, либо занят другим процессом.");
                throw;
            }
        }
        public static void AppendAllFops(Config config, string fopsData)
        {
            try
            {
                File.AppendAllText(config.FilePathOutput, fopsData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
