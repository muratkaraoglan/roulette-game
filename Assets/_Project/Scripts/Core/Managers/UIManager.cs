using _Project.Scripts.GamePlay.BetSystem;
using _Project.Scripts.Utils.NumberFormatting;
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
            DataManager.Instance.BetDataService.OnBetPlaced += OnBetPlaced;
            DataManager.Instance.BetDataService.OnBetRemoved += OnBetPlaced;
            DataManager.Instance.MoneyService.OnMoneyChanged += OnTotalMoneyChanged;
            DataManager.Instance.BetDataService.OnAllBetsCleared += BetDataServiceOnOnAllBetsCleared;
            totalBetText.text = DataManager.Instance.BetDataService.GetTotalBetAmount().ToString();
            OnTotalMoneyChanged(DataManager.Instance.MoneyService.TotalMoney);
        }

        private void BetDataServiceOnOnAllBetsCleared()
        {
            totalBetText.text = "0";
        }


        private void OnDisable()
        {
            DataManager.Instance.BetDataService.OnBetPlaced -= OnBetPlaced;
            DataManager.Instance.BetDataService.OnBetRemoved -= OnBetPlaced;
            DataManager.Instance.MoneyService.OnMoneyChanged -= OnTotalMoneyChanged;
        }

        private void OnTotalMoneyChanged(int totalMoney)
        {
            totalMoneyText.text = totalMoney.ToString();
        }

        private void OnBetPlaced(Bet bet)
        {
            var totalBetAmount = DataManager.Instance.BetDataService.GetTotalBetAmount();
            totalBetText.text = totalBetAmount.ToString();
        }
    }
}