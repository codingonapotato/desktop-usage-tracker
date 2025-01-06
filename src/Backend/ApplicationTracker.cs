namespace Backend;
using System.Runtime.Versioning;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
    private Dictionary<string, Application> _Tracked = new Dictionary<string, Application>();
    private Dictionary<string, Application> _Untracked = new Dictionary<string, Application>(); // tracked at some point in the past during the lifetime of this object
    private Application? ForegroundApplication = null; // Either a tracked application or null

    [DllImport("User32.dll")] static extern IntPtr GetForegroundWindow();
    [DllImport("User32.dll")] static extern IntPtr SetWindowsHookExA(int idHook, IntPtr lpfn, IntPtr hmod, uint dwThreadId);

    public delegate IntPtr HookProc(int nCode, UIntPtr wParam, IntPtr lParam);
    private static HookProc _HookProcDelegate = ForegroundWindowCallback;

    private const int WH_CBT = 5;

    /// <summary>
    /// Callback function for events emitted by the user's ForegroundWindow
    /// </summary>
    /// <returns> IntPtr </returns>
    private static IntPtr ForegroundWindowCallback(int nCode, UIntPtr wParam, IntPtr lParam)
    {
        return IntPtr.Zero; //stub
    }

    /// <summary>
    /// Instantiate ApplicationTracker object
    /// </summary>
    /// <list type="bullet">
    /// <item> Subscribe to foreground window changes (TODO: implement event handler for this) </item>
    /// </list>
    /// <returns> void </returns>
    public ApplicationTracker()
    {
        IntPtr hookProcPtr = Marshal.GetFunctionPointerForDelegate(_HookProcDelegate);
        SetWindowsHookExA(WH_CBT, hookProcPtr, 0, (uint)Process.GetCurrentProcess().Id);
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
        // Set untracked application to tracked and move entry to tracked dictionary
        if (_Untracked.ContainsKey(name) && state)
        {
            _Untracked[name].SetTracked(state);
            _Tracked.Add(name, _Untracked[name]);
            _Untracked.Remove(name);
        }
        // Set tracked application to untracked and move entry to untracked dictionary
        else if (_Tracked.ContainsKey(name) && !state)
        {
            _Tracked[name].SetTracked(state);
            _Untracked.Add(name, _Tracked[name]);
            _Tracked.Remove(name);
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
        // TODO: Handle ArgumentException from Application constructor
        return; // stub
    }

    /// <summary>
    /// Modify the category given the name of an application
    /// </summary>
    /// <param name="name"></param>
    /// <returns> void </returns>
    public void ModifyApplicationCategory(string name)
    {
        // TODO: Handle possible exception from Application.ModifyCategory
        return; //stub
    }

}
