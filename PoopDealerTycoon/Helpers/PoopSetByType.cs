using System.Collections;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PoopSetByType
    {
        private static Dictionary<PoopType, PoopSet> _poopSetByTypeDict = new Dictionary<PoopType, PoopSet>{
            {PoopType.NormalPoop, PoopSet.A},
            {PoopType.EmojiPoop, PoopSet.A},
            {PoopType.DogPoop, PoopSet.A},
            {PoopType.PinkPoop, PoopSet.B},
            {PoopType.IceCreamPoop, PoopSet.B},
            {PoopType.DonutPoop, PoopSet.B},
            {PoopType.RainbowPoop, PoopSet.C},
            {PoopType.GoldPoop, PoopSet.C},
            {PoopType.DiamondPoop, PoopSet.C}
        };

        public static PoopSet GetPoopSetByType(PoopType poopType)
        {
            return _poopSetByTypeDict[poopType];
        }
    }

}