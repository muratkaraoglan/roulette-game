using _Project.Scripts.GamePlay.BetSystem;
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
            DataManager.Instance.betDataService.OnBetPlaced += OnBetPlaced;
        }

        private void OnDisable()
        {
            DataManager.Instance.betDataService.OnBetPlaced -= OnBetPlaced;
        }

        private void OnBetPlaced(Bet bet)
        {
            var totalBetAmount = DataManager.Instance.betDataService.GetTotalBetAmount();
            totalBetText.text = totalBetAmount.ToString();
        }
    }
}