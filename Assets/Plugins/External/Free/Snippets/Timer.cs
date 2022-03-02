// Credit: https://github.com/SebLague/Solar-System/blob/Development/Assets/Scripts/Game/Utility/Timer.cs

using System.Collections.Generic;
using System.Diagnostics;

public static class Timer
{
    private static Dictionary<string, Stopwatch> timers;

    static Timer()
    {
        if (timers == null)
        {
            timers = new Dictionary<string, Stopwatch>();
        }
    }

    public static void Start(string name)
    {
        if (timers.ContainsKey(name))
        {
            UnityEngine.Debug.Log($"Timer ({name}) already exists");
        }
        else
        {
            timers.Add(name, Stopwatch.StartNew());
        }
    }

    public static void Stop(string name)
    {
        if (timers.ContainsKey(name))
        {
            UnityEngine.Debug.Log($"Timer ({name}) {timers[name].ElapsedMilliseconds} ms");
            timers.Remove(name);
        }
        else
        {
            UnityEngine.Debug.Log($"Timer ({name}) does not exist");
        }
    }
}