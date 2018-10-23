using System.Linq;
using System.Collections.Generic;

using FSBO.Installer.Models;

namespace FSBO.Installer.Instances
{
    public class AreaInstances : IInstances<AreaConfig>
    {
        private static List<AreaConfig> Areas;

        private static AreaConfig ExampleArea = new AreaConfig() { Name = "Example", Value = "value" };

        static AreaInstances()
        {
            var instances = typeof(AreaInstances)
                .GetFields()
                .Where(f => f.FieldType == typeof(AreaConfig))
                .Select(f => (AreaConfig)f.GetValue(null))
                .ToList();

            Areas = instances;
        }


        public List<AreaConfig> GetInstances()
        {
            return Areas;
        }
    }
}
