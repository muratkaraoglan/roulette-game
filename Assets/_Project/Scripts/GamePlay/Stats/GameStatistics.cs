using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GamePlay.Stats
{
    [Serializable]
    public class GameStatistics
    {
        public int totalSpins;
        public float totalBetAmount;
        public float totalWinAmount;
        public float netProfit;
        public List<NumberFrequency> numberFrequencyList;
        public float winRate;
        public int longestWinStreak;
        public int longestLoseStreak;
        public int currentStreak;
        public bool isWinStreak;

        [System.NonSerialized]
        private Dictionary<int, int> _numberFrequency;
        
        public Dictionary<int, int> NumberFrequency => _numberFrequency;

        public GameStatistics()
        {
            numberFrequencyList = new List<NumberFrequency>();
        }

        public void InitializeFrequencyDictionary()
        {
            _numberFrequency = new Dictionary<int, int>();
        
            // European roulette: 0-36 (37 numbers total)
            for (int i = 0; i <= 36; i++)
            {
                _numberFrequency[i] = 0;
            }
        
            if (numberFrequencyList is { Count: > 0 })
            {
                foreach (var freq in numberFrequencyList)
                {
                    if (freq.number is >= 0 and <= 36)
                    {
                        _numberFrequency[freq.number] = freq.frequency;
                        Debug.Log($"Loaded: Number {freq.number} = {freq.frequency} times");
                    }
                }
            }
            else
            {
                Debug.Log("No frequency data found in numberFrequencyList");
            }
        }

        public void UpdateFrequencyList()
        {
            numberFrequencyList.Clear();
            if (_numberFrequency != null)
            {
                foreach (var kvp in _numberFrequency)
                {
                    numberFrequencyList.Add(new NumberFrequency(kvp.Key, kvp.Value));
                }
            }
        }

    }

    [Serializable]
    public struct NumberFrequency
    {
        public int number;
        public int frequency;

        public NumberFrequency(int num, int freq)
        {
            number = num;
            frequency = freq;
        }
    }
}