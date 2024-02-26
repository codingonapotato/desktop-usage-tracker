using System.ComponentModel;
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
    public Category Category;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <param name="process"> TODO </param>
    public Application(Process process)
    {
        _Process = process;
        Tracked = false;
        StartTime = new DateTime();
        Category = Category.GetInstance(Category.DEFAULT_NAME);
    }
    
    public int GetProcessId()
    {
        return 0;    // stub
    }

    public DateTime CalculateTimeElapsed(DateTime endTime)
    {
        return new DateTime();    // stub
    }
}