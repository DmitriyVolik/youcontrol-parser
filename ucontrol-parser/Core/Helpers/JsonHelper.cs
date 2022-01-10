using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace selenium_test.Core.Helpers
{
    public static class JsonHelper<T>
    {
        public static T JsonToObject(string jsonData)
        {
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        public static string ObjectToJson(T jsonObject)
        {
            var settings = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(jsonObject, settings);
        }
    }
}
