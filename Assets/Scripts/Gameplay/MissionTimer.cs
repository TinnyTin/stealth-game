using System;
using System.Diagnostics;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 */

public class MissionTimer
{
    private readonly Stopwatch _stopwatch = new(); 

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void Reset()
    {
        _stopwatch.Reset();
    }

    public string GetTimeHoursMinsSecsMillis()
    {
        TimeSpan ts = _stopwatch.Elapsed;
        return ts.ToString(@"hh:\mm\:ss\.fff"); 
    }

    public string GetTimeMinsSecsMillis()
    {
        TimeSpan ts = _stopwatch.Elapsed;
        return ts.ToString(@"mm\:ss\.fff");
    }

    public string GetTimeCustomFormat(string format)
    {
        TimeSpan ts = _stopwatch.Elapsed;
        return ts.ToString(format);
    }

    public TimeSpan GetTimespan()
    {
        return _stopwatch.Elapsed; 
    }
}