using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

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

    /// <summary>
    /// Adds a new valid defined category for all Application instances to use 
    /// </summary>
    /// <param name="category"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void AddCategory(string category)
    {
        if (!IsValidCategory(category))
        {
            throw new ArgumentException("Category must be between 1 - 50 characters and cannot be all white-space characters");
        }
        else
        {
            _DefinedCategories.Add(category.ToLower());
        }

    }

    /// <summary>
    /// Modifies an existing defined category for all Application instances  
    /// </summary>
    /// <remarks>
    /// <param name="oldCategory"> must be defined and <param name="newCategory"> must be valid. Otherwise this function does nothing
    /// </remarks>
    /// <param name="oldCategory">Name referencing the category to change</param>
    /// <param name="newCategory">New name to update the category to</param>
    /// <exception cref="ArgumentException"></exception>
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