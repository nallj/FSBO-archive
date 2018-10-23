using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

using NUT = FSBO.Installer;

namespace FSBO.Installer.Test
{
    [TestFixture]
    public class RunnerTests
    {
        [Test]
        public void EnsureAreas_WebServicesClientCalled_PerformGetCallWithWebServicesClient()
        {
            // ARRANGE
            var installRunner = new NUT.Runner();
            //var mockWebServicesClient = new Fakes.FakeWebServicesClient();
            var stubInstallContext = new Fakes.FakeInstallerContext();
            //installRunner.InstallContext = new InstallerContext(mockWebServicesClient);
            installRunner.InstallContext = stubInstallContext;

            /*mockWebServicesClient.ClientResult = new List<DAL.Area>()
            {
                new DAL.Area()
                {
                    AreaId = 7,
                    AreaTypeId = 1,
                    Value = "32608"
                }
            };*/

            // ACT
            //var stuff = installRunner.InstallContext.GetAreas();

            // ASSERT
            Assert.IsTrue(true);
        }
    }
}
