using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class SplitBetArea : BaseBetArea, IBetAreaInteractable
    {
        public void OnSelect()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, true);
        }

        public void OnDeselect()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, false);
        }
        
        public void TryPlaceBet()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseTryPlaceChipEvent(transform, betRule.PayoutMultiplier,
                betRule.CoveredNumbers);
        }
    }
}