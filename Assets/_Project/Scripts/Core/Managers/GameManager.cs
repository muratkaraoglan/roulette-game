using System;
using System.Collections;
using _Project.Scripts.Core.Event;
using _Project.Scripts.Core.Helper;
using _Project.Scripts.Utils;
using UnityEngine;

namespace _Project.Scripts.Core.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [field: SerializeField] public GameState GameState { get; private set; } = GameState.WaitingForBets;

        private void OnEnable()
        {
            GameEventManager.Instance.RouletteEvents.OnSpinStart += OnSpinStart;
            GameEventManager.Instance.RouletteEvents.OnSpinComplete += OnSpinComplete;
        }

        private void OnSpinComplete(int obj)
        {
            GameState = GameState.ShowingResult;
            StartCoroutine(DelayedGameStateToBet());
        }

        private void OnSpinStart()
        {
            GameState = GameState.SpinningWheel;
        }

        IEnumerator DelayedGameStateToBet()
        {
            yield return Extension.GetWaitForSeconds(2f);
            GameState = GameState.WaitingForBets;
        }
    }

    public enum GameState
    {
        WaitingForBets,
        SpinningWheel,
        ShowingResult,
    }
}