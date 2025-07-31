using System;
using System.Globalization;

namespace _Project.Scripts.GamePlay.Stats
{
    [Serializable]
    public class SpinRecord
    {
        public int spinNumber;
        public int resultNumber;
        public int totalBetAmount;
        public int totalWinAmount;
        public int netResult;
        public string timestamp;
        public SpinRecord(int spin, int result, int bet, int win)
        {
            spinNumber = spin;
            resultNumber = result;
            totalBetAmount = bet;
            totalWinAmount = win;
            netResult = win - bet;
            timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }
    }
}