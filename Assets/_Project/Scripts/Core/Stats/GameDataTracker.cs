using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Helper;
using _Project.Scripts.Core.Services;
using UnityEngine;

namespace _Project.Scripts.Core.Stats
{
    public class GameDataTracker : Singleton<GameDataTracker>
    {
        private List<SpinRecord> _spinHistory = new List<SpinRecord>();
        private GameStatistics _currentStats = new GameStatistics();
        private ISaveService<StatsSerializeData> _saveService;
        [SerializeField] private StatsSerializeData _stats;
        public GameStatistics CurrentStats => _currentStats;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _saveService = new JsonFileDataServıce<StatsSerializeData>("GameStatistics.json");
            _stats = _saveService.Load();
            _spinHistory = _stats.spinHistory;
            _currentStats = _stats.gameStatistics;
        }

        public void RecordSpin(int resultNumber, int betAmount, int winAmount)
        {
            int spinNumber = _spinHistory.Count + 1;
            var spinRecord = new SpinRecord(spinNumber, resultNumber, betAmount, winAmount);
            _spinHistory.Add(spinRecord);
            UpdateStatics(spinRecord);
            SaveData();
        }

        private void UpdateStatics(SpinRecord record)
        {
            _currentStats.totalSpins++;
            _currentStats.totalBetAmount += record.totalBetAmount;
            _currentStats.totalWinAmount += record.totalWinAmount;
            _currentStats.netProfit = _currentStats.totalWinAmount - _currentStats.totalBetAmount;

            _currentStats.NumberFrequency[record.resultNumber]++;

            int wins = _spinHistory.Count(s => s.netResult > 0);
            _currentStats.winRate = (float)wins / _currentStats.totalSpins * 100f;

            UpdateStreaks(record.netResult > 0);
            _currentStats.UpdateFrequencyList();
        }

        private void UpdateStreaks(bool isWin)
        {
            if (isWin)
            {
                if (_currentStats.isWinStreak)
                {
                    _currentStats.currentStreak++;
                }
                else
                {
                    _currentStats.isWinStreak = true;
                    _currentStats.currentStreak = 1;
                }

                if (_currentStats.currentStreak > _currentStats.longestWinStreak)
                {
                    _currentStats.longestWinStreak = _currentStats.currentStreak;
                }
            }
            else
            {
                if (!_currentStats.isWinStreak)
                {
                    _currentStats.currentStreak++;
                }
                else
                {
                    _currentStats.isWinStreak = false;
                    _currentStats.currentStreak = 1;
                }

                if (_currentStats.currentStreak > _currentStats.longestLoseStreak)
                {
                    _currentStats.longestLoseStreak = _currentStats.currentStreak;
                }
            }
        }

        public void ResetSession()
        {
            _spinHistory.Clear();
            _currentStats = new GameStatistics();
            SaveData();
        }

        public Dictionary<int, float> GetNumberProbabilities()
        {
            Dictionary<int, float> probabilities = new Dictionary<int, float>();

            if (_currentStats.totalSpins == 0)
            {
                var defaultProbability = 1f / 37f;
                for (int i = 0; i <= 36; i++)
                {
                    probabilities[i] = defaultProbability;
                }
            }
            else
            {
                for (int i = 0; i <= 36; i++)
                {
                    probabilities[i] = (float)_currentStats.NumberFrequency[i] / _currentStats.totalSpins;
                }
            }

            return probabilities;
        }

        public List<int> GetHotNumbers(int count = 5)
        {
            return _currentStats.NumberFrequency
                .OrderByDescending(kvp => kvp.Value)
                .Take(count)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        public List<int> GetColdNumbers(int count = 5)
        {
            return _currentStats.NumberFrequency
                .OrderBy(kvp => kvp.Value)
                .Take(count)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        public float GetZeroHitRate()
        {
            if (_currentStats.totalSpins == 0) return 0f;
            return (float)_currentStats.NumberFrequency[0] / _currentStats.totalSpins * 100f;
        }

        void SaveData()
        {
            _stats.gameStatistics = CurrentStats;
            _stats.spinHistory = _spinHistory;
            _saveService.Save(_stats);
        }

        void LoadData()
        {
        }
    }
}