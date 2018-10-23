using System.Collections.Generic;

namespace FSBO.Installer.Instances
{
    public interface IInstances<InstanceType>
    {
        List<InstanceType> GetInstances();
    }
}
