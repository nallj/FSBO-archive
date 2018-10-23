using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FSBO.DAL;

namespace FSBO.Installer.Test.Fakes
{
    public class FakeInstallerContext : IInstallerContext
    {
        public List<Area> Areas { get; set; }

        public FakeInstallerContext()
        {

        }


        public List<Area> GetAreas()
        {
            return new List<Area>();
        }
    }
}
