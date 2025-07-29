using System.Collections;
using System.Linq;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class StraightBetArea : BaseBetArea, IBetAreaInteractable
    {
        [SerializeField] private GameObject highlightGameObject;

        private void OnEnable()
        {
            GameEventManager.Instance.BetAreaEvents.HighlightBetAreaEvent += HighlightBetAreaEvent;
            GameEventManager.Instance.RouletteEvents.OnSpinComplete += OnSpinComplete;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.BetAreaEvents.HighlightBetAreaEvent -= HighlightBetAreaEvent;
            GameEventManager.Instance.RouletteEvents.OnSpinComplete -= OnSpinComplete;
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


        private void OnSpinComplete(int targetNumber)
        {
            var contain = betRule.CoveredNumbers.Contains(targetNumber);
            if (contain) StartCoroutine(HighlightAnimation());
        }
        
        IEnumerator HighlightAnimation()
        {
            ChangeHighlight(true);
            yield return Extension.GetWaitForSeconds(TimeConstant.BetAreaResultAnimationTime);
            ChangeHighlight(false);
        }

        private void ChangeHighlight(bool highlightState) => highlightGameObject.SetActive(highlightState);

        public void OnMouseDown()
        {
            ChangeHighlight(true);
        }

        public void OnMouseUp()
        {
            ChangeHighlight(false);
        }

        public void TryPlaceBet()
        {
            GameEventManager.Instance.BetAreaEvents.RaiseTryPlaceChipEvent(transform, betRule.PayoutMultiplier,
                betRule.CoveredNumbers);
        }
    }
}