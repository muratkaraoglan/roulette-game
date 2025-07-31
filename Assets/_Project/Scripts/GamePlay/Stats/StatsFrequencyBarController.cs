using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GamePlay.Stats
{
    public class StatsFrequencyBarController : MonoBehaviour
    {
        [SerializeField] private Image frequencyBar;
        [SerializeField] private TextMeshProUGUI rateText;
        [SerializeField] private TextMeshProUGUI idText;

        public void Init(int id)
        {
            idText.text = id.ToString();
        }
        
        public void SetBarAmount(float amount, string rate)
        {
            frequencyBar.fillAmount = amount;
            rateText.text = rate;
        }
    }
}