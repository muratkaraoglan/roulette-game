using System;

namespace _Project.Scripts.Core.Event
{
    public class GameStatsEvent
    {
        public Action OnStatsReady;
        public void RaiseOnStatsReady() => OnStatsReady?.Invoke();
        
        public Action OnStatsUpdated;
        public void RaiseOnStatsUpdated() => OnStatsUpdated?.Invoke();
    }
}