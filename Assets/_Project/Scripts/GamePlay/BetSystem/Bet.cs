using _Project.Scripts.GamePlay.ChipSystem;
using UnityEngine;

namespace _Project.Scripts.GamePlay.BetSystem
{
    [System.Serializable]
    public class Bet
    {
        public string betAreaName;
        public ChipEnum chip;
        public int amount;
        public int payoutMultiplier;
        public int[] coveredNumbers;
        public Vector3 position;
        public Bet(string betAreaName, int amount, int payoutMultiplier, int[] coveredNumbers, ChipEnum chip, Vector3 position)
        {
            this.betAreaName = betAreaName;
            this.amount = amount;
            this.payoutMultiplier = payoutMultiplier;
            this.coveredNumbers = coveredNumbers;
            this.chip = chip;
            this.position = position;
        }
    }
}