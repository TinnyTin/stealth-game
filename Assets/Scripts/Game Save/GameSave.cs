using System;
using System.Collections.Generic;

public class GameSave
{
    public class LevelStat
    {
        public int LevelNumber { get; set; }
        public TimeSpan BestTime { get; set; }
    }

    public int UnlockedToLevel { get; set; }
    public List<LevelStat> LevelStats { get; set; } = new(); 
}