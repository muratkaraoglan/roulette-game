using System;
using UnityEngine;

namespace _Project.Scripts.Core.Event
{
    public class BetAreaEvents
    {
        public Action<int[], bool> HighlightBetAreaEvent;
        public void RaiseBetAreaHighlightEvent(int[] coveredNumbers, bool highlightState) =>
            HighlightBetAreaEvent?.Invoke(coveredNumbers, highlightState);

        public Action<Transform, int, int[]> TryPlaceChip;
        public void RaiseTryPlaceChipEvent(Transform chipParent, int payoutMultiplier, int[] coveredNumbers) =>
            TryPlaceChip?.Invoke(chipParent, payoutMultiplier, coveredNumbers);

        public Action<int> OnBetPlaced;
        public void RaiseOnBetPlaced(int totalBetAmount) => OnBetPlaced?.Invoke(totalBetAmount);
    }
}