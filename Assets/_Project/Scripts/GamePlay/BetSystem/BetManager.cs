using System.Collections.Generic;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Managers;
using _Project.Scripts.GamePlay.ChipSystem;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class BetManager : MonoBehaviour
    {
        [SerializeField] private int money = 500;

        private readonly Dictionary<string, int> _betAreasCount = new();

        private void OnEnable()
        {
            GameEventManager.Instance.BetAreaEvents.TryPlaceChip += TryPlaceChip;
        }

        private void Start()
        {
            InitializeBets();
        }

        private void OnDisable()
        {
            GameEventManager.Instance.BetAreaEvents.TryPlaceChip -= TryPlaceChip;
        }

        private void InitializeBets()
        {
            var bets = DataManager.Instance.betDataService.GetAllBets();
            foreach (var bet in bets)
            {
                var currentCount = _betAreasCount.GetValueOrDefault(bet.betAreaName, 0);
                currentCount++;
                _betAreasCount[bet.betAreaName] = currentCount;
                ChipManager.Instance.InstantiateChip(bet.chip, bet.position);
            }
            bets.Clear();
        }

        private void TryPlaceChip(Transform betArea, int payoutMultiplier, int[] coveredNumbers)
        {
            var selectedChip = ChipManager.Instance.GetSelectedChip();
            if ((int)selectedChip <= money)
            {
                _betAreasCount.TryGetValue(betArea.name, out var betCount);
                betCount++;
                _betAreasCount[betArea.name] = betCount;
                var position = betArea.position;
                position.y += betCount * .1f;
                position.z += betCount * .03f;
                var bet = new Bet(betArea.name, (int)selectedChip, payoutMultiplier, coveredNumbers, selectedChip,
                    position);
                DataManager.Instance.betDataService.PlaceBet(bet);
                //money -= (int)selectedChip;
                ChipManager.Instance.InstantiateChip(selectedChip, position);
            }
            else
            {
                print("chip could not be placed");
            }
        }
    }
}