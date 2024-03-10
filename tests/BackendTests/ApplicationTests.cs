namespace BackendTests;

using System.Diagnostics;
using System.Runtime.Versioning;
using Backend;
using NUnit.Framework.Internal;

public class ApplicationTests
{
    // [SetUp]
    // public void Setup()
    // {
    //     return;    //stub
    // }

    [TestFixture]
    class TryGetParentProcessTests : ApplicationTests
    {
        /// <summary>
        /// Precondition: An instance of the process being tested MUST be running before running this test case
        /// </summary>
        [Test]
        [SupportedOSPlatform("windows")]
        public void ProcessFound()
        {
            Process? parentProcess;
            Assert.True(Application.TryGetParentProcess("chrome", out parentProcess));
            if (parentProcess == null)
                Assert.Fail("Expected to have a Process object instance here. Perhaps the pre-condition for the test case has not been satisfied?");
            else
                Assert.True(parentProcess.ProcessName.ToLower() == "chrome");
        }

        [Test]
        [SupportedOSPlatform("windows")]
        public void ProcessNotFound()
        {
            Process? parentProcess;
            Assert.False(Application.TryGetParentProcess("not a real process name", out parentProcess));
            Assert.True(parentProcess == null);
        }
    }

    [TestFixture]
    class ConstructorTests
    {
        /// <summary>
        /// Precondition: An instance of the process being tested MUST be running before running this test case
        /// </summary>
        [Test]
        public void TestConstructorFields()
        {
            Application applicationInstance = new Application("chrome");
            //TODO: Check fields



        }
    }
}