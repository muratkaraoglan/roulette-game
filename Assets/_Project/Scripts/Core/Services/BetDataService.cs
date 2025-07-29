using System;
using System.Collections.Generic;
using _Project.Scripts.GamePlay.BetSystem;

namespace _Project.Scripts.Core.Services
{
    public class BetDataService
    {
        private readonly List<Bet> _bets = new();

        public event Action<Bet> OnBetPlaced;
        public event Action<Bet> OnBetRemoved;
        public event Action OnAllBetsCleared;

        public BetDataService(List<Bet> bets)
        {
            _bets = bets;
        }

        public void PlaceBet(Bet bet)
        {
            _bets.Add(bet);
            OnBetPlaced?.Invoke(bet);
        }

        public bool RemoveBet(Bet bet)
        {
            bool removed = _bets.Remove(bet);
            if (removed)
            {
                OnBetRemoved?.Invoke(bet);
            }

            return removed;
        }

        public List<Bet> GetAllBets()
        {
            return new List<Bet>(_bets);
        }

        public int GetTotalBetAmount()
        {
            int total = 0;
            foreach (var bet in _bets)
            {
                total += bet.amount;
            }

            return total;
        }

        public void ClearAllBets()
        {
            _bets.Clear();
            OnAllBetsCleared?.Invoke();
        }
    }
}