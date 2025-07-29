using System;

namespace _Project.Scripts.Core.Event
{
    public class RouletteEvents
    {
        public Action<int> OnSpinComplete;
        public void RaiseSpinComplete(int value) => OnSpinComplete?.Invoke(value);
    }
}