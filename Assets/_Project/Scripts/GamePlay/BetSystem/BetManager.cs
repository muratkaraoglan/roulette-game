using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Event;
using _Project.Scripts.GamePlay.ChipSystem;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    public class BetManager : MonoBehaviour
    {
        [SerializeField] private int money = 500;
        [SerializeField] private List<Bet> bets = new();
        
        private readonly Dictionary<string, int> _betAreasCount = new();

        private void OnEnable()
        {
            GameEventManager.Instance.BetAreaEvents.TryPlaceChip += TryPlaceChip;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.BetAreaEvents.TryPlaceChip -= TryPlaceChip;
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
                bets.Add(bet);
                var totalBet = bets.Sum(b => b.amount);
                //money -= (int)selectedChip;
                GameEventManager.Instance.BetAreaEvents.RaiseOnBetPlaced(totalBet);
                ChipManager.Instance.InstantiateChip(selectedChip, position);
            }
            else
            {
                print("chip could not be placed");
            }
        }
    }
}