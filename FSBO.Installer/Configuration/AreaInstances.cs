using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBO.Installer.Configuration
{
    public static class AreaInstances
    {
        public static Dictionary<string, AreaTypeConfig> AreaTypesDictionary { get; set; }

        public static List<AreaConfig> Areas { get; set; }

        static AreaInstances()
        {
            AreaTypesDictionary = new Dictionary<string, AreaTypeConfig>()
            {
                { "Postal Code", new AreaTypeConfig("Postal Code") }
            };

            Areas = new List<AreaConfig>()
            {
                new AreaConfig(AreaTypesDictionary["AreaTypesDictionary"], areaValue: "33406"),
                new AreaConfig(AreaTypesDictionary["AreaTypesDictionary"],
                    areaValueRange: Enumerable.Range(32607, 32608).Select(s => s.ToString()))
            };
        }
    }
}
