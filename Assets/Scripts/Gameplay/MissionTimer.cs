using System;
using System.Diagnostics;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 */

public class MissionTimer
{
    private readonly Stopwatch _stopwatch = new(); 
    private TimeSpan _lastElapsed = TimeSpan.Zero; 

    /// <summary>
    /// Starts the stopwatch. It will continue from its last stop
    /// point, which may be zero. 
    /// </summary>
    public void Start()
    {
        _stopwatch.Start();
    }

    /// <summary>
    /// Stops the stopwatch. The elapsed time will be recorded. 
    /// </summary>
    public void Stop()
    {
        _stopwatch.Stop();
        _lastElapsed = _stopwatch.Elapsed; 
    }

    /// <summary>
    /// Resets the stop watch. The recorded elapsed time is reset to zero. 
    /// </summary>
    public void Reset()
    {
        _stopwatch.Reset();
        _lastElapsed = TimeSpan.Zero;
    }

    /// <summary>
    /// Records the elapsed time of the stop watch and returns the elapsed
    /// time as a string in hh:mm:ss.fff format. 
    /// </summary>
    /// <returns>Elapsed time string in hh:mm:ss.fff format.</returns>
    public string GetElapsedTimeHoursMinsSecsMillis()
    {
        _lastElapsed = _stopwatch.Elapsed; 
        return GetElapsedTimeCustomFormat(@"hh:\mm\:ss\.fff"); 
    }

    /// <summary>
    /// Records the elapsed time of the stop watch and returns the elapsed
    /// time as a string in mm:ss.fff format. 
    /// </summary>
    /// <returns>Elapsed time string in mm:ss.fff format.</returns>
    public string GetElapsedTimeMinsSecsMillis()
    {
        _lastElapsed = _stopwatch.Elapsed;
        return GetElapsedTimeCustomFormat(@"mm\:ss\.fff");
    }

    /// <summary>
    /// Records the elapsed time of the stop watch and returns the elapsed
    /// time in a custom format. See TimeSpan documentation for valid formats. 
    /// </summary>
    /// <returns>Elapsed time string in custom format.</returns>
    public string GetElapsedTimeCustomFormat(string format)
    {
        TimeSpan ts = _stopwatch.Elapsed;
        return ts.ToString(format);
    }

    /// <summary>
    /// Records the current elapsed time of the stopwatch. 
    /// </summary>
    /// <returns>Current elapsed stopwatch time as TimeSpan.</returns>
    public TimeSpan GetElapsedTime()
    {
        _lastElapsed = _stopwatch.Elapsed;
        return _lastElapsed; 
    }

    /// <summary>
    /// Returns the last recorded elapsed time of the stopwatch without
    /// taking a new reading. 
    /// </summary>
    /// <returns>Last recorded elapsed stopwatch time as TimeSpan.</returns>
    public TimeSpan GetLastElapsedTime()
    {
        return _lastElapsed; 
    }

    /// <summary>
    /// Returns the running state of the stopwatch. This can be useful when
    /// determining if you should use GetElapsedTime() or GetLastElapsedTime()
    /// when taking a reading from the stopwatch. 
    /// </summary>
    /// <returns>The running state of the stopwatch.</returns>
    public bool IsRunning()
    {
        return _stopwatch.IsRunning; 
    }
}