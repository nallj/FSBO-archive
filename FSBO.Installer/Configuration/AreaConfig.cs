using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBO.Installer.Configuration
{
    public class AreaConfig
    {
        public AreaTypeConfig AreaType { get; set; }

        public string AreaValue { get; set; }

        public IEnumerable<string> AreaValueRange { get; set; }

        public AreaConfig(AreaTypeConfig areaType, string areaValue = null, IEnumerable<string> areaValueRange = null)
        {
            AreaType = areaType;
            AreaValue = areaValue;
            AreaValueRange = areaValueRange;
        }
    }
}
