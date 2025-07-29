using System;
using System.Collections.Generic;
using System.IO;
using _Project.Scripts.Core.Helper;
using _Project.Scripts.Core.Services;
using _Project.Scripts.GamePlay.BetSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private Data data;
        public BetDataService BetDataService;
        public MoneyService MoneyService;
        private string _path;

        protected override void Awake()
        {
            _path = Path.Combine(Application.persistentDataPath, "Data.json");
            data = new Data();
            Configure(config =>
            {
                config.Lazy = true;
                config.DestroyOthers = true;
                config.Persist = true;
            });
            base.Awake();
            if (LoadData() == null)
            {
                data = new Data();
                data.totalMoney = 5000;
                data.bets = new();
            }

            BetDataService = new BetDataService(data.bets);
            MoneyService = new MoneyService(data.totalMoney);
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveData();
            }
        }

        private void SaveData()
        {
            data.bets = BetDataService.GetAllBets();
            data.totalMoney = MoneyService.TotalMoney;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_path, json);
        }

        private Data LoadData()
        {
            if (!File.Exists(_path)) return null;
            string json = File.ReadAllText(_path);
            data = JsonUtility.FromJson<Data>(json);
            return data;
        }
    }

    [Serializable]
    public class Data
    {
        public int totalMoney;
        public List<Bet> bets;
    }
}