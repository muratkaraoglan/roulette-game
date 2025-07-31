using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Project.Scripts.Core.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GamePlay.Stats
{
    public class StatsUIManager : MonoBehaviour
    {
        [SerializeField] private StatsFrequencyBarController statsFrequencyBarControllerPrefab;
        [SerializeField] private Transform statsContainer;
        [SerializeField] private GameObject statsHolder;
        [SerializeField] private TextMeshProUGUI statsText;

        [SerializeField] private Button showStatsButton;
        [SerializeField] private Button hideStatsButton;

        private readonly Dictionary<int, StatsFrequencyBarController> _statsFrequencyBarControllers = new();
        private readonly StringBuilder _stringBuilder = new();

        private void OnEnable()
        {
            GameEventManager.Instance.GameStatsEvent.OnStatsReady += OnStatsReady;
            GameEventManager.Instance.GameStatsEvent.OnStatsUpdated += OnStatsUpdated;
        }

        private void Start()
        {
            showStatsButton.onClick.AddListener(OnShowStatsButtonClicked);
            hideStatsButton.onClick.AddListener(OnHideStatsButtonClicked);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.GameStatsEvent.OnStatsReady -= OnStatsReady;
            GameEventManager.Instance.GameStatsEvent.OnStatsUpdated -= OnStatsUpdated;
        }

        private void OnStatsReady()
        {
            var numberProb = GameDataTracker.Instance.GetNumberProbabilities();
            for (int i = 0; i < numberProb.Count; i++)
            {
                var statsController = Instantiate(statsFrequencyBarControllerPrefab, statsContainer);
                statsController.Init(i);
                _statsFrequencyBarControllers.Add(i, statsController);
                SetProbabilities(i, numberProb[i]);
            }

            _statsFrequencyBarControllers[0].transform.SetSiblingIndex(numberProb.Count - 1);
            SetStatsText();
        }

        private void OnStatsUpdated()
        {
            var numberProb = GameDataTracker.Instance.GetNumberProbabilities();
            foreach (var kvp in numberProb)
            {
                SetProbabilities(kvp.Key, kvp.Value);
            }

            SetStatsText();
        }

        private void SetProbabilities(int id, float probability)
        {
            var statsController = _statsFrequencyBarControllers[id];
            _stringBuilder.Clear();
            if (probability > 0f)
            {
                _stringBuilder.AppendFormat("{0:0.0}%", probability * 100f);
            }

            statsController.SetBarAmount(probability, _stringBuilder.ToString());
        }

        private void SetStatsText()
        {
            _stringBuilder.Clear();
            _stringBuilder.AppendFormat("Total Spins: {0}", GameDataTracker.Instance.CurrentStats.totalSpins)
                .AppendLine()
                .AppendFormat("Total Bet Amount: {0}", GameDataTracker.Instance.CurrentStats.totalBetAmount)
                .AppendLine()
                .AppendFormat("Total Win Amount: {0}", GameDataTracker.Instance.CurrentStats.totalWinAmount)
                .AppendLine()
                .AppendFormat("Profit: {0}", GameDataTracker.Instance.CurrentStats.netProfit)
                .AppendLine()
                .AppendFormat("Win Rate: {0:0.0}%", GameDataTracker.Instance.CurrentStats.winRate)
                .AppendLine()
                .AppendFormat("Longest Win Streak: {0}", GameDataTracker.Instance.CurrentStats.longestWinStreak)
                .AppendLine()
                .AppendFormat("Longest Lose Streak: {0}", GameDataTracker.Instance.CurrentStats.longestLoseStreak)
                .AppendLine()
                .AppendFormat("Current Streak: {0}", GameDataTracker.Instance.CurrentStats.currentStreak)
                .AppendLine()
                .AppendFormat("Hot Numbers: {0}", string.Join(", ", GameDataTracker.Instance.GetHotNumbers()))
                .AppendLine()
                .AppendFormat("Cold Numbers: {0}", string.Join(", ", GameDataTracker.Instance.GetColdNumbers()));


            statsText.text = _stringBuilder.ToString();
        }

        private void OnShowStatsButtonClicked()
        {
            statsHolder.SetActive(true);
        }

        private void OnHideStatsButtonClicked()
        {
            statsHolder.SetActive(false);
        }
    }
}