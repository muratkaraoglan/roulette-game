namespace _Project.Scripts.GamePlay.BetSystem
{
    [System.Serializable]
    public class Bet
    {
        public int amount;
        public int payoutMultiplier;
        public int[] coveredNumbers;

        public Bet(int amount, int payoutMultiplier, int[] coveredNumbers)
        {
            this.amount = amount;
            this.payoutMultiplier = payoutMultiplier;
            this.coveredNumbers = coveredNumbers;
        }
    }
}