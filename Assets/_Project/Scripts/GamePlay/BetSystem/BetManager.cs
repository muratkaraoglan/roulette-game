using System.Collections.Generic;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Managers;
using _Project.Scripts.GamePlay.ChipSystem;
using _Project.Scripts.GamePlay.Stats;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class BetManager : MonoBehaviour
    {
        private readonly Dictionary<string, int> _betAreasCount = new();
        private const float ChipYOffset = .1f;
        private const float ChipZOffset = .03f;
        
        private void OnEnable()
        {
            GameEventManager.Instance.BetAreaEvents.TryPlaceChip += TryPlaceChip;
            GameEventManager.Instance.RouletteEvents.OnSpinComplete += OnSpinComplete;
        }

        private void Start()
        {
            InitializeBets();
        }

        private void OnDisable()
        {
            GameEventManager.Instance.BetAreaEvents.TryPlaceChip -= TryPlaceChip;
            GameEventManager.Instance.RouletteEvents.OnSpinComplete -= OnSpinComplete;
        }

        private void InitializeBets()
        {
            var bets = DataManager.Instance.BetDataService.GetAllBets();
            foreach (var bet in bets)
            {
                var currentCount = _betAreasCount.GetValueOrDefault(bet.betAreaName, 0);
                currentCount++;
                _betAreasCount[bet.betAreaName] = currentCount;
                ChipManager.Instance.InstantiateChip(bet.chip, bet.position);
            }

            bets.Clear();
        }

        private void OnSpinComplete(int targetNumber)
        {
            var newAmount = DataManager.Instance.BetDataService.CalculateAllBetAmount(targetNumber);
            DataManager.Instance.MoneyService.AddMoney(newAmount);
            GameDataTracker.Instance.RecordSpin(targetNumber, DataManager.Instance.BetDataService.GetTotalBetAmount(),newAmount);
            DataManager.Instance.BetDataService.ClearAllBets();
            _betAreasCount.Clear();
        }

        private void TryPlaceChip(Transform betArea, int payoutMultiplier, int[] coveredNumbers)
        {
            var selectedChip = ChipManager.Instance.GetSelectedChip();
            if (DataManager.Instance.MoneyService.HasEnough((int)selectedChip))
            {
                _betAreasCount.TryGetValue(betArea.name, out var betCount);
                betCount++;
                _betAreasCount[betArea.name] = betCount;
                var position = betArea.position;
                position.y += betCount * ChipYOffset;
                position.z += betCount * ChipZOffset;
                var bet = new Bet(betArea.name, (int)selectedChip, payoutMultiplier, coveredNumbers, selectedChip,
                    position);
                DataManager.Instance.BetDataService.PlaceBet(bet);
                DataManager.Instance.MoneyService.SubtractMoney((int)selectedChip);
                ChipManager.Instance.InstantiateChip(selectedChip, position);
            }
            else
            {
                print("chip could not be placed");
            }
        }
    }
}