namespace Backend;

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
    public bool Tracked; // TODO - Setter for this needs to be more complicated... If set to False, need to also set the EndTime field to current time. Otherwise, set the StartTime to current time
    public DateTime StartTime;
    public DateTime EndTime;
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
        TryGetParentProcess(name, out Process);
        Name = GetProductName(Process);
        Tracked = false;
        StartTime = DateTime.Now;
        EndTime = DateTime.MinValue;
        Elapsed = TimeSpan.Zero;
        Category = Category.GetInstance(Category.DEFAULT_NAME);
    }

    /// <summary>
    /// Attempt to retrieve the parent process for a running executable with the specified name and returns true. 
    /// Returns false if no such process exists.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool TryGetParentProcess(string name, out Process? parentProcess)
    {
        Process[] processes = Process.GetProcessesByName(name);
        if (processes.Length == 0)
        {
            parentProcess = null;
            return false;
        }
        else if (processes.Length == 1)    // Case where only parent process exists
        {
            parentProcess = processes[0];
            return true;
        }

        else
        {
            // Query WMI to get the ParentProcessId for 2 processes
            (int processId, uint parentProcessId)[] parentProcessIds = new (int, uint)[2];
            for (int i = 0; i < 2; i++)
            {
                string query = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", processes[i].Id);
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection results = managementObjectSearcher.Get();
                ManagementObjectCollection.ManagementObjectEnumerator enumerator = results.GetEnumerator();
                enumerator.MoveNext();
                parentProcessIds[i] = (processes[i].Id, (uint)enumerator.Current["ParentProcessId"]);
            }

            // Find the true parent process id. Assumes that processes with the same name cannot have different parent processes
            if (parentProcessIds[0].parentProcessId == parentProcessIds[1].parentProcessId)    // Case 1: Both proccesses have the same parent
                parentProcess = Process.GetProcessById((int)parentProcessIds[0].parentProcessId);
            else
            {
                if (parentProcessIds[0].parentProcessId == parentProcessIds[1].processId)   // Case 2: Process 1 is process 0's parent
                    parentProcess = Process.GetProcessById((int)parentProcessIds[0].parentProcessId);
                else
                    parentProcess = Process.GetProcessById((int)parentProcessIds[1].parentProcessId);    // Case 3: Process 0 is process 1's parent
            }
            return true;
        }
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
    /// Calculates the amount of time elapsed for the application so far and updates the Elapsed field
    /// </summary>
    /// <returns> TimeSpan object containing the time elapsed </returns>
    public TimeSpan CalculateTimeElapsed()
    {
        /*TODO - Incomplete 
          Full requirements:
            - If the application is being tracked, method should:
                1. Calculate the difference between the start and current time 
                2. Add that to the elapsed time
                3. Set the current time as the new "start" time
            - Otherwise:
                1. Return the elapsed time. (Setter for tracker will be responsible for the correct time-elapsed calculation)
          */
        if (Tracked)
        {
            TimeSpan currentTimeElapsed = DateTime.Now.Subtract(StartTime);
            Elapsed = Elapsed.Add(currentTimeElapsed);
            StartTime = DateTime.Now;
        }

        return Elapsed;
    }

    /// <summary>
    /// Changes the category field for this instance to the Category instance with the specified name
    /// </summary>
    /// <remarks>
    /// Requires: name is a valid name for a Category
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
            throw new ArgumentException("Expected an instance of the Process class");
        else
        {
            string defaultName = process.ProcessName;     // Use the process name as a default
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