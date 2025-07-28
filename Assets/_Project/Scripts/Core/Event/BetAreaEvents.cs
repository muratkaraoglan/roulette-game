using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.Event
{
    public class BetAreaEvents
    {
        public Action<int[], bool> HighlightBetAreaEvent;

        public void RaiseBetAreaHighlightEvent(int[] coveredNumbers, bool highlightState) =>
            HighlightBetAreaEvent?.Invoke(coveredNumbers, highlightState);
    }
}