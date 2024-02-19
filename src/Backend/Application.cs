using System.Diagnostics;

/// <summary>
/// Class model for an application
/// </summary>
public class Application
{
    private static HashSet<string> _DefinedCategories = new HashSet<string>();
    private Process _Process;
    public bool Tracked = false;
    private DateTime StartTime { get; set; }
    private DateTime EndTime { get; set; }
    public string Category = string.Empty;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <param name="process"> Process </param>
    public Application(Process process)
    {
        _Process = process;
    }
}