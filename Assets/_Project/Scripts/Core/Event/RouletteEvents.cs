using System;

namespace _Project.Scripts.Core.Event
{
    public class RouletteEvents
    {
        public Action OnSpinStart;
        public void RaiseSpinStart() => OnSpinStart?.Invoke();

        public Action<int> OnSpinComplete;
        public void RaiseSpinComplete(int value) => OnSpinComplete?.Invoke(value);
    }
}