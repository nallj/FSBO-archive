using System.Collections.Generic;

namespace FSBO.Installer
{
    public interface IInstallerContext
    {
        void WriteLine(string text, params object[] args);
        void Warn(string text, params object[] args);
        void Error(string text, bool isFatal = false, params object[] args);

        //List<DAL.Area> GetAreas();
    }
}
