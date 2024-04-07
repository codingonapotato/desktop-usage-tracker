namespace BackendTests;

using System.Diagnostics;
using System.Threading;
using System.Runtime.Versioning;
using Backend;
using NUnit.Framework.Internal;

[SupportedOSPlatform("windows")]
public class ApplicationTests
{
    ProcessStartInfo startInfo = new ProcessStartInfo(@"..\..\..\iperf-3.1.3-win64\iperf3.exe", "-s");
    string processName = "iperf3";
    public void cleanUpProcessRoutine(Process process)
    {
        process.Kill();
        process.WaitForExit();
        process.Close();
    }

    [TestFixture]
    class TryGetParentProcessTests
    {
        /// <summary>
        /// Precondition: An instance of the process being tested MUST be running before running this test case
        /// </summary>
        [Test]
        public void ProcessFound()
        {
            Process? parentProcess;
            Assert.True(Application.TryGetMainProcess("chrome", out parentProcess));
            if (parentProcess == null)
                Assert.Fail("Expected to have a Process object instance here. Perhaps the pre-condition for the test case has not been satisfied?");
            else
                Assert.True(parentProcess.ProcessName.ToLower() == "chrome");
        }

        /// <summary>
        /// Precondition: An instance of the process being tested MUST NOT be running before running this test case
        /// </summary>
        [Test]
        public void ProcessNotFound()
        {
            Process? parentProcess;
            Assert.False(Application.TryGetMainProcess("not a real process name", out parentProcess));
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
            // Check Elapsed field
            Assert.True(applicationInstance.Elapsed == TimeSpan.Zero);
            // Check Category field
            Assert.True(applicationInstance.Category == Category.GetInstance(Category.DEFAULT_NAME));
        }
    }

    [TestFixture]
    class IsApplicationRunningTests : ApplicationTests
    {
        /// <summary>
        /// Pre-condition: 
        ///  No instance of iperf3 is running before executing this test </item>
        /// </summary>
        [Test]
        public void ApplicationIsRunning()
        {
            Process? process = Process.Start(startInfo);
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                Assert.True(applicationInstance.IsApplicationRunning());

                cleanUpProcessRoutine(process);

                Assert.False(applicationInstance.IsApplicationRunning());
            }
        }
    }

    [TestFixture]
    class CalculateTimeElapsedTests : ApplicationTests
    {
        /// <summary>
        /// Pre-condition: 
        ///  No instance of Window's iperf3 is running before executing this test </item>
        /// </summary>
        [Test]
        public void SimpleTimeElapsedForRunningApplication()
        {
            Process? process = Process.Start(startInfo);
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.SetTracked(true);
                TimeSpan firstElapsed = applicationInstance.CalculateTimeElapsed();
                Thread.Sleep(1000);
                TimeSpan secondElapsed = applicationInstance.CalculateTimeElapsed();

                cleanUpProcessRoutine(process);

                Assert.True(secondElapsed.CompareTo(firstElapsed) > 0);
            }
        }

        /// <summary>
        /// Pre-condition: 
        /// No instance of Window's iperf3 is running before executing this test </item>
        /// </summary>
        [Test]
        public void SimpleTimeElapsedForExitedApplication()
        {
            Process? process = Process.Start(startInfo);
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.SetTracked(true);

                cleanUpProcessRoutine(process);

                TimeSpan timeElapsedBeforeSleep = applicationInstance.CalculateTimeElapsed();
                Thread.Sleep(1000);
                TimeSpan timeElapsedAfterSleep = applicationInstance.CalculateTimeElapsed();
                Assert.True(timeElapsedAfterSleep.CompareTo(timeElapsedBeforeSleep) == 0);    // check that time elapsed after 1-second sleep has not increased
            }
        }

        /// <summary>
        /// Pre-condition: 
        /// No instance of Window's iperf3 is running before executing this test </item>
        /// </summary>
        [Test]
        public void TimeElapsedForDynamicallyTrackedAppliation()
        {
            Process? process = Process.Start(startInfo);
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.SetTracked(true);
                Thread.Sleep(1000);    //  1-second passes while application is tracked
                applicationInstance.SetTracked(false);
                TimeSpan timeElapsedTracked = applicationInstance.CalculateTimeElapsed();
                Thread.Sleep(1000);    //  1-second passes while application is un-tracked
                applicationInstance.SetTracked(true);
                Thread.Sleep(1000);    //  1-second passes while application is tracked
                TimeSpan timeElapsedRetracked = applicationInstance.CalculateTimeElapsed();

                Assert.True(timeElapsedTracked.CompareTo(timeElapsedRetracked) < 0);    // Check that time elapsed of re-tracked application is greater

                cleanUpProcessRoutine(process);
            }
        }

    }
}