using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class LineBetArea : BaseBetArea, IBetAreaInteractable
    {
        public void OnMouseDown()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, true);
        }

        public void OnMouseUp()
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