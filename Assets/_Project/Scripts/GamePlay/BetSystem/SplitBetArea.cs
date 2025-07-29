using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class SplitBetArea : BaseBetArea, IBetAreaInteractable
    {
        public void OnMouseDown()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, true);
        }

        public void OnMouseUp()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, false);
            //check can place bet
        }

        public bool TryPlaceBet()
        {
            //Bet manager try place
            // send possible position for placement
            return false;
        }
    }
}