using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class StreetBetArea : BaseBetArea,IBetAreaInteractable
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
            throw new System.NotImplementedException();
        }
    }
}