using _Project.Scripts.Core.Event;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class LineBetArea : BaseBetArea
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
            return false;
        }
    }
}