namespace BackendTests;

using System.Diagnostics;
using System.Threading;
using System.Runtime.Versioning;
using Backend;

[SupportedOSPlatform("windows")]
public class ApplicationTests
{
    ProcessStartInfo startInfo = new ProcessStartInfo(@"..\..\..\iperf-3.1.3-win64\iperf3.exe", "-s");
    Process? process;
    string processName = "iperf3";

    [SetUp]
    public void BeforeEach()
    {
        process = Process.Start(startInfo);
    }

    [TearDown]
    public void CleanUpProcessRoutine()
    {
        if (process is null || process.HasExited) return;

        process.Kill();
        process.WaitForExit();
        process.Close();
        process = null;
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
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                Assert.True(applicationInstance.IsApplicationRunning());

                CleanUpProcessRoutine();

                Assert.False(applicationInstance.IsApplicationRunning());
            }
        }
    }

    [TestFixture]
    class SetTrackedTests : ApplicationTests
    {
        /// <summary>
        /// Toggle SetTracked for an Application object instance and confirm internal object state
        /// </summary>
        [Test]
        public void SetTracked()
        {
            if (process is null)
                Assert.Fail(
                    "Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.SetTracked(false);
                Assert.False(applicationInstance.Tracked);
                applicationInstance.SetTracked(true);
                Assert.True(applicationInstance.Tracked);
                Assert.True(applicationInstance.ReferenceTime.CompareTo(DateTime.Now) <= 0);
            }
        }
    }

    [TestFixture]
    class ModifyCategoryTests : ApplicationTests
    {
        [Test]
        public void ModifyCategory()
        {
            if (process is null)
                Assert.Fail(
                    "Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.ModifyCategory("Productivity");
                Assert.True(applicationInstance.Category == Category.GetInstance("Productivity"));

                try
                {
                    applicationInstance.ModifyCategory("");
                    Assert.Fail("Expected previous line to throw ArgumentException");
                }
                catch (ArgumentException)
                {
                    Assert.True(applicationInstance.Category == Category.GetInstance("Productivity"));   // Application instance is unchanged for an invalid category
                }
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
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.SetTracked(true);
                TimeSpan firstElapsed = applicationInstance.CalculateTimeElapsed();
                Thread.Sleep(1000);
                TimeSpan secondElapsed = applicationInstance.CalculateTimeElapsed();

                CleanUpProcessRoutine();

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
            if (process is null)
                Assert.Fail("Expecting a running process. Perhaps the path is wrong or the process trying to be run does not exist on the local machine");
            else
            {
                Application applicationInstance = new Application(processName);
                applicationInstance.SetTracked(true);

                CleanUpProcessRoutine();

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

            }
        }

    }
}