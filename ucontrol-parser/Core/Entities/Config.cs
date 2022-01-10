using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_test.Core.Entities
{
    [Serializable]
    public class Config
    {
        public string FilePathInput { get; set; } = "input.txt";
        public string FilePathOutput { get; set; } = "fops.txt";
        public string CurrentCity { get; set; } = "";
        public int Counter { get; set; } = 1;
        public bool IsProxy { get; set; } = false;
        public List<string> ProxyList { get; set; } = new List<string>()
        {
            "134.249.185.223:41890"
        };
    }
}
