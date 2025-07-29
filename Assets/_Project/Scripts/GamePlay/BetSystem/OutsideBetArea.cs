using System.Collections;
using System.Linq;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Interact;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class OutsideBetArea : BaseBetArea, IBetAreaInteractable
    {
        [SerializeField] private GameObject highlightGameObject;

        private void OnEnable()
        {
            GameEventManager.Instance.RouletteEvents.OnSpinComplete += OnSpinComplete;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.RouletteEvents.OnSpinComplete -= OnSpinComplete;
        }

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
        
        private void OnSpinComplete(int targetNumber)
        {
            var contain = betRule.CoveredNumbers.Contains(targetNumber);
            if (contain) StartCoroutine(HighlightAnimation());
        }

        IEnumerator HighlightAnimation()
        {
            highlightGameObject.SetActive(true);
            yield return Extension.GetWaitForSeconds(TimeConstant.BetAreaResultAnimationTime);
            highlightGameObject.SetActive(false);
        }
    }
}