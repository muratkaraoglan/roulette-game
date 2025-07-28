using System.Linq;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class StraightBetArea : BaseBetArea, IBetAreaInteractable
    {
        [SerializeField] private GameObject highlightGameObject;

        private void OnEnable()
        {
            GameEventManager.Instance.BetAreaEvents.HighlightBetAreaEvent += HighlightBetAreaEvent;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.BetAreaEvents.HighlightBetAreaEvent -= HighlightBetAreaEvent;
        }

        private void HighlightBetAreaEvent(int[] coveredNumbers, bool highlightState)
        {
            if (!highlightState)
            {
                ChangeHighlight(false);
                return;
            }

            var contain = coveredNumbers.Contains(betRule.CoveredNumbers[0]);
            ChangeHighlight(contain);
        }

        private void ChangeHighlight(bool highlightState) => highlightGameObject.SetActive(highlightState);

        public void OnMouseDown()
        {
            ChangeHighlight(true);
        }

        public void OnMouseUp()
        {
            ChangeHighlight(false);
            //check can place bet
        }

        public bool TryPlaceBet()
        {
            throw new System.NotImplementedException();
        }
    }
}