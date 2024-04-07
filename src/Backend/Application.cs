namespace Backend;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Runtime.Versioning;

/// <summary>
/// Class model for an application
/// </summary>
[SupportedOSPlatform("windows")]
public class Application
{
    public Process? Process;
    public string Name;
    private bool _Tracked;
    public bool Tracked
    {
        get { return _Tracked; }
    }
    public DateTime ReferenceTime;
    public TimeSpan Elapsed;
    public Category Category;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <remarks>
    /// Pre-condition: "name" parameter must correspond to the name of a running process
    /// </remarks>
    /// <param name="name"> Name of a running executable </param>
    public Application(string name)
    {
        TryGetMainProcess(name, out Process);
        Name = GetProductName(Process);
        _Tracked = false;
        ReferenceTime = DateTime.Now;
        Elapsed = TimeSpan.Zero;
        Category = Category.GetInstance(Category.DEFAULT_NAME);
    }

    /// <summary>
    /// Attempt to retrieve the main process for a running executable with the specified name and returns true. 
    /// Returns false if no such process exists.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool TryGetMainProcess(string name, out Process? mainProcess)
    {
        Process[] processes = Process.GetProcessesByName(name);
        if (processes.Length == 0)
            mainProcess = null;
        else if (processes.Length == 1)    // Case where only the main process exists
            mainProcess = processes[0];
        else
        {
            // Query ProcessId, CreationDate, and ExecutablePath from WMI with the matching executable name and add them to ArrayList
            ArrayList processList = new ArrayList();
            string query = string.Format("SELECT ProcessId, ExecutablePath, CreationDate FROM Win32_Process WHERE Name = '{0}.exe'", name);
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection results = managementObjectSearcher.Get();
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = results.GetEnumerator();
            while (enumerator.MoveNext())
                processList.Add(((uint)enumerator.Current["ProcessId"], (string)enumerator.Current["ExecutablePath"], (string)enumerator.Current["CreationDate"]));

            // Find the process with the earliest creation time and a non-null executable path which will be the id of the main process
            uint earliestProcessId = 0;
            DateTime earliestCreationDate = DateTime.MaxValue;
            foreach (ValueTuple<uint, string, string> processTuple in processList)
            {
                string processExecutablePath = processTuple.Item2;
                if (processExecutablePath == string.Empty)
                    continue;
                string creationDate = processTuple.Item3;    // Example: 20240331142353.658251
                int year = int.Parse(creationDate.Substring(0, 4));
                int month = int.Parse(creationDate.Substring(4, 2));
                int day = int.Parse(creationDate.Substring(6, 2));
                int hour = int.Parse(creationDate.Substring(8, 2));
                int minutes = int.Parse(creationDate.Substring(10, 2));
                int seconds = int.Parse(creationDate.Substring(12, 2));
                int milliseconds = int.Parse(creationDate.Substring(15, 3));
                int microseconds = int.Parse(creationDate.Substring(18, 3));

                DateTime processCreationDateTime = new DateTime(year, month, day, hour, minutes, seconds, milliseconds, microseconds);

                if (processCreationDateTime.CompareTo(earliestCreationDate) < 0)
                {
                    earliestCreationDate = processCreationDateTime;
                    earliestProcessId = processTuple.Item1;
                }
            }

            mainProcess = Process.GetProcessById((int)earliestProcessId);
        }
        return mainProcess is not null;
    }

    /// <summary>
    /// Set the value of the tracked field to the specified state and made necessary modifications to Application interal state
    /// </summary>
    /// <param name="state"></param>
    public void SetTracked(bool state)
    {
        if (state)
            ReferenceTime = DateTime.Now;
        _Tracked = state;
    }

    /// <summary>
    /// Returns whether the application instance is associated with a running process
    /// </summary>
    /// <remarks> If the application is not running (not associated with a process) this function will return false </remarks>
    /// <returns> bool </returns>
    public bool IsApplicationRunning()
    {
        if (Process is null)
            return false;
        Process.Refresh();
        return !Process.HasExited;
    }

    /// <summary>
    /// Calculates the amount of time elapsed for the application so far and updates the Elapsed field if the application is still running
    /// </summary>
    /// <returns> TimeSpan object containing the time elapsed </returns>
    public TimeSpan CalculateTimeElapsed()
    {
        if (Tracked && IsApplicationRunning())
        {
            TimeSpan currentTimeElapsed = DateTime.Now.Subtract(ReferenceTime);
            Elapsed = Elapsed.Add(currentTimeElapsed);
            ReferenceTime = DateTime.Now;
        }

        return Elapsed;
    }

    /// <summary>
    /// Changes the category field for this instance to the Category instance with the specified name
    /// </summary>
    /// <remarks>
    /// Requires: name is a valid input. Valid inputs must:
    /// <list type="number">
    /// <item>
    /// Be between 1 and 50 characters (inclusive) in length
    /// </item>
    /// <item>
    /// Not a string with only whitespace characters
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    public void ModifyCategory(string name)
    {
        Category = Category.GetInstance(name);
    }

    /// <summary>
    /// Retrieves the product name of a process
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static string GetProductName(Process? process)
    {
        if (process is null)
            throw new ArgumentException("Expected an instance of the Process");
        else
        {
            string defaultName = process.ProcessName;     // Use the process name as a default
            try
            {
                ProcessModule? processModule = process.MainModule;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return defaultName;
            }
            if (process.MainModule is null)
                return defaultName;
            else
            {
                string fileName = process.MainModule.FileName;
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
                return fileVersionInfo.ProductName ?? defaultName;
            }
        }
    }
}