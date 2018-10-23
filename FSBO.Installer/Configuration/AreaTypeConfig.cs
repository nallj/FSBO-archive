using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBO.Installer.Configuration
{
    public class AreaTypeConfig
    {
        public string AreaTypeName { get; set; }

        public AreaTypeConfig(string areaTypeName)
        {
            AreaTypeName = areaTypeName;
        }
    }
}
