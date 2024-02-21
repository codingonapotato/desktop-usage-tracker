using System.Diagnostics;

/// <summary>
/// Class model for an application
/// </summary>
public class Application
{
    private static HashSet<string> _DefinedCategories = new HashSet<string>();
    public static HashSet<string> DefinedCategories
    {
        get { return _DefinedCategories; }
        set { _DefinedCategories = value; }
    }
    private Process _Process;
    public bool Tracked = false;
    private DateTime _StartTime;
    private DateTime StartTime { get; set; }
    private DateTime EndTime { get; set; }
    public string Category = string.Empty;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <param name="process"> TODO </param>
    public Application(Process process)
    {
        _Process = process;
    }

    public static void AddCategory(string category)
    {
        return;   // stub
    }

    public static void ModifyCategory(string category)
    {
        return;   // stub
    }

    public static void RemoveCategory(string category)
    {
        return;   // stub
    }

    public static void IsValidCategory(string category)
    {
        return;   // stub
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