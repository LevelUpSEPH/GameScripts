using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class EarnedMoneyController
    {
        public static int CalculateTotalEarnedMoney(PoopBase[] poops)
        {
            int totalEarnedMoney = 0;
            foreach(PoopBase poop in poops)
            {
                totalEarnedMoney += poop.GetPoopSellPrice();
            }
            return totalEarnedMoney;
        } 
    }
}