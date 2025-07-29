using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class OutsideBetArea : BaseBetArea, IBetAreaInteractable
    {
        [SerializeField] private GameObject highlightGameObject;

        public void OnMouseDown()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, true);
            highlightGameObject.SetActive(true);
        }

        public void OnMouseUp()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseBetAreaHighlightEvent(betRule.CoveredNumbers, false);
            highlightGameObject.SetActive(false);
        }
        
        public void TryPlaceBet()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseTryPlaceChipEvent(transform, betRule.PayoutMultiplier,
                betRule.CoveredNumbers);
        }
    }
}