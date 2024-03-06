namespace Backend;
using System.Diagnostics;

/// <summary>
/// Class model for an application
/// </summary>
public class Application
{
    private Process? _Process;
    public string Name;
    public bool Tracked;
    public DateTime StartTime;
    public DateTime EndTime;
    public TimeSpan Elapsed;
    public Category Category;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <param name="name"> Name of a running executable </param>
    public Application(string name)
    {
        Name = name;
        _Process = null;
        Tracked = false;
        StartTime = DateTime.Now;
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

        else
        {
            string query = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", processes[0]);
            ManagementObjectSearcher
        }
    }

    /// <summary>
    /// Returns whether the application instance is associated with a running process
    /// </summary>
    /// <remarks> If the application is not running (not associated with a process) this function will return false </remarks>
    /// <returns> bool </returns>
    public bool IsApplicationRunning()
    {
        if (_Process is null)
            return false;
        _Process.Refresh();
        return _Process.HasExited;
    }

    /// <summary>
    /// Returns the process id for a running application
    /// </summary>
    /// <returns></returns>
    public int GetProcessId()
    {
        return _Process.Id;
    }

    /// <summary>
    /// Calculates the amount of time elapsed for the application so far
    /// </summary>
    /// <remarks> Requires: endTime is always chronologically later than Application.StartTime </remarks>
    /// <param name="endTime"></param>
    /// <returns> TimeSpan object containing the time elapsed </returns>
    public TimeSpan CalculateTimeElapsed(DateTime endTime)
    {
        if (Tracked)
            return endTime.Subtract(StartTime);
        else
            return EndTime.Subtract(StartTime);
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
}