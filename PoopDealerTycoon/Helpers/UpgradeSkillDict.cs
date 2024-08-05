using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Upgrade;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class UpgradeSkillDict
    {
        public static Dictionary<UpgradeSkillName, UpgradeSkill> upgradableSkillsDict = new Dictionary<UpgradeSkillName, UpgradeSkill>()
        {
            {UpgradeSkillName.PlayerSpeed, UpgradeSkillFactory.playerSpeed},
            {UpgradeSkillName.PlayerStack, UpgradeSkillFactory.playerStack},

            {UpgradeSkillName.WorkerASpeed, UpgradeSkillFactory.workerASpeed},
            {UpgradeSkillName.WorkerAStack, UpgradeSkillFactory.workerAStack},
            {UpgradeSkillName.WorkerBSpeed, UpgradeSkillFactory.workerBSpeed},
            {UpgradeSkillName.WorkerBStack, UpgradeSkillFactory.workerBStack},
            {UpgradeSkillName.WorkerCSpeed, UpgradeSkillFactory.workerCSpeed},
            {UpgradeSkillName.WorkerCStack, UpgradeSkillFactory.workerCStack},

            {UpgradeSkillName.NormalPoopSellPrice, UpgradeSkillFactory.normalPoopSellPrice},
            {UpgradeSkillName.EmojiPoopSellPrice, UpgradeSkillFactory.emojiPoopSellPrice},
            {UpgradeSkillName.DogPoopSellPrice, UpgradeSkillFactory.dogPoopSellPrice},

            {UpgradeSkillName.PinkPoopSellPrice, UpgradeSkillFactory.pinkPoopSellPrice},
            {UpgradeSkillName.IceCreamPoopSellPrice, UpgradeSkillFactory.iceCreamPoopSellPrice},
            {UpgradeSkillName.DonutPoopSellPrice, UpgradeSkillFactory.donutPoopSellPrice},

            {UpgradeSkillName.RainbowPoopSellPrice, UpgradeSkillFactory.rainbowPoopSellPrice},
            {UpgradeSkillName.GoldPoopSellPrice, UpgradeSkillFactory.goldPoopSellPrice},
            {UpgradeSkillName.DiamondPoopSellPrice, UpgradeSkillFactory.diamondPoopSellPrice}
        };
    }
}