namespace Backend;
using System.Runtime.Versioning;
using System.Diagnostics;

/// <summary>
/// Class model for an ApplicationTracker
/// <list type="bullet">
/// <item> Needs to periodically (every minute) update the time elapsed for all tracked applications </item>
/// <item> Needs to be aware of any changes in Application (existing and newly tracked ones) </item>
/// <item> Will instantiate Application objects upon request... (will provide way to get list of running processes in console) </item>
/// </list>
/// </summary>
[SupportedOSPlatform("windows")]
public class ApplicationTracker
{
    private Dictionary<string, Application> Tracked = new Dictionary<string, Application>();
    private Dictionary<string, Application> Untracked = new Dictionary<string, Application>(); // tracked at some point in the past during the lifetime of this object
    private Application? ForegroundApplication = null; // Either a tracked application or null

    /// <summary>
    /// Instantiate ApplicationTracker object
    /// </summary>
    /// <list type="bullet">
    /// <item> Subscribe to foreground window changes (TODO: implement event handler for this) </item>
    /// </list>
    /// <returns> void </returns>
    public ApplicationTracker()
    {
        return; // stub
    }

    /// <summary>
    /// Updates time elapsed for the foregroundApplication (if it is not null)
    /// </summary>
    /// <param name="interval"></param>
    /// <returns> void </returns>
    public void UpdateTimeElapsed()
    {
        if (ForegroundApplication == null) return;
        ForegroundApplication.CalculateTimeElapsed();
    }

    /// <summary>
    /// Updates the Tracked field for an application to the specified state
    /// </summary>
    /// <param name="name"></param>
    /// <returns> void </returns>
    public void SetTracked(string name, bool state)
    {
        if (Untracked.ContainsKey(name) && state)
        {
            Untracked[name].SetTracked(state);
            Tracked.Add(name, Untracked[name]);
            Untracked.Remove(name);
        }
        else if (Tracked.ContainsKey(name) && !state)
        {
            Tracked[name].SetTracked(state);
            Untracked.Add(name, Tracked[name]);
            Tracked.Remove(name);
        }
    }

    /// <summary>
    /// Add a new application to be tracked 
    /// </summary>
    /// <list type="bullet">
    /// <item> Create new Application object </item>
    /// <item> Check DB to see if this application was tracked before today... if yes then overwrite the time elapsed </item>
    /// <item> Subscribe ApplicationTracker to the relevant process events (e.g. exit)... will need event handler for this </item>
    /// <item> Add new Application to tracked dictionary w/ process name as the key </item>
    /// </list>
    /// <param name="name"></param>
    /// <returns> void </returns>
    public void AddApplication(string name)
    {
        return; // stub
    }

    /// <summary>
    /// Modify the category given the name of an application
    /// </summary>
    /// <param name="name"></param>
    /// <returns> void </returns>
    public void ModifyApplicationCategory(string name)
    {
        return; // stub
    }

}
