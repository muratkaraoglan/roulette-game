using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.Stats
{
    [Serializable]
    public class StatsSerializeData
    {
        public List<SpinRecord> spinHistory = new();
        public GameStatistics gameStatistics = new();
    }
}