using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    [CreateAssetMenu(fileName = "BetRule-", menuName = "Roulette/New Bet Rule", order = 0)]
    public class BetRuleSO : ScriptableObject
    {
        [field: SerializeField,Min(1)] public int PayoutMultiplier { get; private set; } = 1;
        [field: SerializeField] public int[] CoveredNumbers { get; private set; }
        
    }
}