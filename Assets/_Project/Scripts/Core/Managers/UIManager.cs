using System;
using _Project.Scripts.Core.Event;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalMoneyText;
        [SerializeField] private TextMeshProUGUI totalBetText;

        private void OnEnable()
        {
            GameEventManager.Instance.BetAreaEvents.OnBetPlaced += OnBetPlaced;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.BetAreaEvents.OnBetPlaced -= OnBetPlaced;
        }

        private void OnBetPlaced(int totalBetAmount)
        {
            totalBetText.text = totalBetAmount.ToString();
        }
    }
}