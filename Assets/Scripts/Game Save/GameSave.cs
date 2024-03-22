using System;
using System.Collections.Generic;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:
 */

public class GameSave
{
    public class LevelStat
    {
        public int LevelNumber { get; set; } = 0;
        public TimeSpan BestTime { get; set; } = TimeSpan.Zero;
    }

    public int UnlockedToLevel { get; set; } = 0; 
    public List<LevelStat> LevelStats { get; set; } = new(); 
}