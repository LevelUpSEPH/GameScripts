using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Upgrade;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class UpgradableSkillFactory
    {
        // Initializ every skill type here
        public static List<UpgradeSkill.UpgradableSkill> playerSpeed = new List<UpgradeSkill.UpgradableSkill>(){
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(50, 1.25f)},
            {new UpgradeSkill.UpgradableSkill(350, 1.45f)}
        };

        public static List<UpgradeSkill.UpgradableSkill> playerStack = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 3)},
            {new UpgradeSkill.UpgradableSkill(0, 4)},
            {new UpgradeSkill.UpgradableSkill(250, 5)},
            {new UpgradeSkill.UpgradableSkill(450, 6)}
        };

        public static List<UpgradeSkill.UpgradableSkill> workerASpeed = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(60, 1.15f)},
        };

        public static List<UpgradeSkill.UpgradableSkill> workerAStack = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(80, 2)},
            {new UpgradeSkill.UpgradableSkill(250, 3)}
        };

        public static List<UpgradeSkill.UpgradableSkill> workerBSpeed = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(120, 1.15f)},
        };

        public static List<UpgradeSkill.UpgradableSkill> workerBStack = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(150, 2)},
            {new UpgradeSkill.UpgradableSkill(500, 3)}
        };

        public static List<UpgradeSkill.UpgradableSkill> workerCSpeed = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(250, 1.15f)},
        };

        public static List<UpgradeSkill.UpgradableSkill> workerCStack = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(320, 2)},
            {new UpgradeSkill.UpgradableSkill(900, 3)}
        };

        public static List<UpgradeSkill.UpgradableSkill> normalPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(20, 2)},
            {new UpgradeSkill.UpgradableSkill(150, 3)}
        };

        public static List<UpgradeSkill.UpgradableSkill> emojiPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 2)},
            {new UpgradeSkill.UpgradableSkill(40, 3)},
            {new UpgradeSkill.UpgradableSkill(225, 4)}
        };

        public static List<UpgradeSkill.UpgradableSkill> dogPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 3)},
            {new UpgradeSkill.UpgradableSkill(60, 4)},
            {new UpgradeSkill.UpgradableSkill(300, 5)}
        };

        public static List<UpgradeSkill.UpgradableSkill> pinkPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(30, 2)},
            {new UpgradeSkill.UpgradableSkill(180, 3)}
        };

        public static List<UpgradeSkill.UpgradableSkill> iceCreamPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 2)},
            {new UpgradeSkill.UpgradableSkill(50, 3)},
            {new UpgradeSkill.UpgradableSkill(250, 4)}
        };

        public static List<UpgradeSkill.UpgradableSkill> donutPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 3)},
            {new UpgradeSkill.UpgradableSkill(70, 4)},
            {new UpgradeSkill.UpgradableSkill(400, 5)}
        };

        public static List<UpgradeSkill.UpgradableSkill> rainbowPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 1)},
            {new UpgradeSkill.UpgradableSkill(40, 2)},
            {new UpgradeSkill.UpgradableSkill(225, 3)}
        };

        public static List<UpgradeSkill.UpgradableSkill> goldPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 2)},
            {new UpgradeSkill.UpgradableSkill(60, 3)},
            {new UpgradeSkill.UpgradableSkill(350, 4)}
        };

        public static List<UpgradeSkill.UpgradableSkill> diamondPoopSellPrice = new List<UpgradeSkill.UpgradableSkill>()
        {
            {new UpgradeSkill.UpgradableSkill(0, 3)},
            {new UpgradeSkill.UpgradableSkill(80, 4)},
            {new UpgradeSkill.UpgradableSkill(425, 5)}
        };

    }
}