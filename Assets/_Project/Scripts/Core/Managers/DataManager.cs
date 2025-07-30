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
        public BetDataService BetDataService;
        public MoneyService MoneyService;
        private ISaveService<Data> _saveService;
        private Data _data;

        protected override void Awake()
        {
            Configure(config =>
            {
                config.Lazy = true;
                config.DestroyOthers = true;
                config.Persist = true;
            });
            base.Awake();
            _saveService = new JsonFileDataServıce<Data>("data.json");
            _data = _saveService.Load();
            BetDataService = new BetDataService(_data.bets);
            MoneyService = new MoneyService(_data.totalMoney);
        }

        private void OnApplicationQuit()
        {
            _data.bets = BetDataService.GetAllBets();
            _data.totalMoney = MoneyService.TotalMoney;
            _saveService.Save(_data);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _data.bets = BetDataService.GetAllBets();
                _data.totalMoney = MoneyService.TotalMoney;
                _saveService.Save(_data);
            }
        }
    }

    [Serializable]
    public class Data
    {
        public int totalMoney = 5000;
        public List<Bet> bets;
    }
}