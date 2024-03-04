namespace Backend;
using System.Diagnostics;

/// <summary>
/// Class model for an application
/// </summary>
public class Application
{
    private Process _Process;
    public bool Tracked;
    public DateTime StartTime;
    public DateTime EndTime;
    public TimeSpan Elapsed;
    public Category Category;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <param name="process"> Process component with a process id corresponding to the parent process for a running executable </param>
    public Application(Process process)
    {
        _Process = process;
        Tracked = false;
        StartTime = DateTime.Now;
        Elapsed = TimeSpan.Zero;
        Category = Category.GetInstance(Category.DEFAULT_NAME);
    }

    /// <summary>
    /// Returns whether the application instance is associated with a running process
    /// </summary>
    /// <returns> bool </returns>
    public bool IsApplicationRunning()
    {
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