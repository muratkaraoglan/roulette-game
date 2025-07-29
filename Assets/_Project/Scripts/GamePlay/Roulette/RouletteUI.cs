using System;
using _Project.Scripts.Core.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts.GamePlay.Roulette
{
    public class RouletteUI : MonoBehaviour
    {
        [SerializeField] private RouletteWheel rouletteWheel;
        [SerializeField] private Button spinButton;
        [SerializeField] private TMP_InputField deterministicInputField;
        private const int MinValue = 0;
        private const int MaxValue = 36;

        private void Start()
        {
            deterministicInputField.onEndEdit.AddListener(ValidateTMPInput);
            spinButton.onClick.AddListener(Spin);
        }

        void ValidateTMPInput(string input)
        {
            if (int.TryParse(input, out int value))
            {
                deterministicInputField.text = Mathf.Clamp(value, MinValue, MaxValue).ToString();
            }
            else
            {
                deterministicInputField.text = MaxValue.ToString();
            }
        }

        private void Spin()
        {
            if (!int.TryParse(deterministicInputField.text, out int value))
            {
                value = Random.Range(MinValue, MaxValue);
            }

            print(value);
            rouletteWheel.SpinToNumber(value);
        }
    }
}