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
    public bool Tracked;
    public DateTime StartTime;
    public DateTime EndTime;
    public string Category;

    /// <summary>
    /// Instantiates an application
    /// </summary>
    /// <param name="process"> TODO </param>
    public Application(Process process)
    {
        _Process = process;
        Tracked = false;
        StartTime = new DateTime();
        Category = "Default";
    }

    /// <summary>
    /// Determines whether the input category is valid
    /// </summary>
    /// <remarks>
    /// Valid inputs must:
    /// <list type="number">
    /// <item>
    /// Be between 1 and 50 characters (inclusive) in length
    /// </item>
    /// <item>
    /// Not a string with only whitespace characters
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="category"></param>
    /// <returns> bool </returns>
    public static bool IsValidCategory(string category)
    {
        return 50 >= category.Length && category.Length > 0 &&
        category.Replace(" ", "").Length != 0;
    }

    public static void AddCategory(string category)
    {
        return;   // stub
    }

    public static void ModifyCategory(string oldCategory, string newCategory)
    {
        return;   // stub
    }

    public static void RemoveCategory(string category)
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