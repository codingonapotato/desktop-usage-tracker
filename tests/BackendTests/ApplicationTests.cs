namespace BackendTests;

using System.Diagnostics;
using System.Threading;
using System.Runtime.Versioning;
using Backend;
using NUnit.Framework.Internal;

[SupportedOSPlatform("windows")]
public class ApplicationTests
{
    [TestFixture]
    class TryGetParentProcessTests : ApplicationTests
    {
        /// <summary>
        /// Precondition: An instance of the process being tested MUST be running before running this test case
        /// </summary>
        [Test]
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
        [TestCase("chrome", "Google Chrome")]
        public void TestConstructorFields(string name, string productName)
        {
            Application applicationInstance = new Application(name);

            // Check Name field
            Assert.True(applicationInstance.Name == productName);
            // Check Process field
            if (applicationInstance.Process == null)
                Assert.Fail("Expected to have a Process object instance here. Perhaps the pre-condition for the test case has not been satisfied?");
            else
                Assert.True(applicationInstance.Process.ProcessName.ToLower() == "chrome");
            // Check Tracked field
            Assert.False(applicationInstance.Tracked);
            // Check EndTime field
            Assert.True(applicationInstance.EndTime == DateTime.MinValue);
            // Check Elapsed field
            Assert.True(applicationInstance.Elapsed == TimeSpan.Zero);
            // Check Category field
            Assert.True(applicationInstance.Category == Category.GetInstance(Category.DEFAULT_NAME));
        }
    }

    [TestFixture]
    class IsApplicationRunningTests
    {
        /// <summary>
        /// Pre-condition: 
        ///  No instance of Window's setting is running before executing this test </item>
        /// </summary>
        [Test]
        public void ApplicationIsRunning()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\ImmersiveControlPanel\SystemSettings.exe");
            Process? edge = Process.Start(startInfo);
            if (edge is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application("SystemSettings");
                Assert.True(applicationInstance.IsApplicationRunning());

                edge.Kill();
                edge.WaitForExit();
                edge.Close();
                Assert.False(applicationInstance.IsApplicationRunning());
            }
        }
    }

    [TestFixture]
    class CalculateTimeElapsedTests
    {
        /// <summary>
        /// Pre-condition: 
        ///  No instance of Window's setting is running before executing this test </item>
        /// </summary>
        [Test]
        public void SimpleTimeElapsedForRunningApplication()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\ImmersiveControlPanel\SystemSettings.exe");
            Process? edge = Process.Start(startInfo);
            if (edge is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application("SystemSettings") { Tracked = true };
                TimeSpan firstElapsed = applicationInstance.CalculateTimeElapsed();
                Thread.Sleep(1000);
                TimeSpan secondElapsed = applicationInstance.CalculateTimeElapsed();

                Assert.True(secondElapsed.CompareTo(firstElapsed) > 0);
            }
        }

        /// <summary>
        /// Pre-condition: 
        ///  No instance of Window's setting is running before executing this test </item>
        /// </summary>
        [Test]
        public void SimpleTimeElapsedForExitedApplication()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\ImmersiveControlPanel\SystemSettings.exe");
            Process? edge = Process.Start(startInfo);
            if (edge is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application("SystemSettings") { Tracked = true };

                edge.Kill();
                edge.WaitForExit();
                edge.Close();

                TimeSpan timeElapsedBeforeSleep = applicationInstance.CalculateTimeElapsed();
                Assert.True(timeElapsedBeforeSleep.CompareTo(applicationInstance.Elapsed))
                Assert.True(applicationInstance.EndTime != DateTime.MinValue);    // check end-time is set
                Thread.Sleep(1000);
                TimeSpan timeElapsedAfterSleep = applicationInstance.CalculateTimeElapsed();
                Assert.True(timeElapsedAfterSleep.CompareTo(timeElapsedBeforeSleep) == 0);    // check that time elapsed after 1-second has not increased
            }
        }

        [Test]

    }
}