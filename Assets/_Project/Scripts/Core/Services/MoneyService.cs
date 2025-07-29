using System;

namespace _Project.Scripts.Core.Services
{
    public class MoneyService
    {
        public event Action<int> OnMoneyChanged;

        private int _totalMoney;

        public int TotalMoney => _totalMoney;

        public MoneyService(int totalMoney)
        {
            _totalMoney = totalMoney;
        }

        public void AddMoney(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive");

            _totalMoney += amount;
            OnMoneyChanged?.Invoke(_totalMoney);
        }

        public bool SubtractMoney(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive");

            if (_totalMoney < amount)
                return false;

            _totalMoney -= amount;
            OnMoneyChanged?.Invoke(_totalMoney);
            return true;
        }

        public bool HasEnough(int amount)
        {
            return _totalMoney >= amount;
        }
    }
}