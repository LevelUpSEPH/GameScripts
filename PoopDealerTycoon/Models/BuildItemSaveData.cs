using System;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle
{
    public class BuildItemSaveData
    {
        public List<BuildItemData> buildItemDatas = new List<BuildItemData>();

        public int GetRequiredMoneyLeft(BuildItemType id)
        {
            foreach(BuildItemData buildItemData in buildItemDatas)
            {
                if(buildItemData.id == id)
                {
                    return buildItemData.requiredMoneyLeft;
                } 
            }
            return 0;
        }
        
        [Serializable]
        public class BuildItemData
        {
            public BuildItemType id;
            public int requiredMoneyLeft;
        }
    }
}