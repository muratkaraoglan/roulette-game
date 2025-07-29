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
        [SerializeField] private Data data;
        public BetDataService betDataService;
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
                data.bets = new();
            }

            betDataService = new BetDataService(data.bets);
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
            data.bets = betDataService.GetAllBets();
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