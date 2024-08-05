using Chameleon.Game.ArcadeIdle.Upgrade;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class UpgradeSkillFactory
    {
        public static UpgradeSkill playerSpeed = new UpgradeSkill(UpgradableSkillFactory.playerSpeed, () => PlayerData.Instance.PlayerSpeedLevel, () => PlayerData.Instance.PlayerSpeedLevel++);
        public static UpgradeSkill playerStack = new UpgradeSkill(UpgradableSkillFactory.playerStack, () => PlayerData.Instance.PlayerStackLevel, () => PlayerData.Instance.PlayerStackLevel++);

        public static UpgradeSkill workerASpeed = new UpgradeSkill(UpgradableSkillFactory.workerASpeed, () => PlayerData.Instance.WorkerASpeedLevel, () => PlayerData.Instance.WorkerASpeedLevel++);
        public static UpgradeSkill workerAStack = new UpgradeSkill(UpgradableSkillFactory.workerAStack, () => PlayerData.Instance.WorkerAStackLevel, () => PlayerData.Instance.WorkerAStackLevel++);
        public static UpgradeSkill workerBSpeed = new UpgradeSkill(UpgradableSkillFactory.workerBSpeed, () => PlayerData.Instance.WorkerBSpeedLevel, () => PlayerData.Instance.WorkerBSpeedLevel++);
        public static UpgradeSkill workerBStack = new UpgradeSkill(UpgradableSkillFactory.workerBStack, () => PlayerData.Instance.WorkerBStackLevel, () => PlayerData.Instance.WorkerBStackLevel++);
        public static UpgradeSkill workerCSpeed = new UpgradeSkill(UpgradableSkillFactory.workerCSpeed, () => PlayerData.Instance.WorkerCSpeedLevel, () => PlayerData.Instance.WorkerCSpeedLevel++);
        public static UpgradeSkill workerCStack = new UpgradeSkill(UpgradableSkillFactory.workerCStack, () => PlayerData.Instance.WorkerCStackLevel, () => PlayerData.Instance.WorkerCStackLevel++);

        public static UpgradeSkill normalPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.normalPoopSellPrice, () => PlayerData.Instance.NormalPoopSellLevel, () => PlayerData.Instance.NormalPoopSellLevel++);
        public static UpgradeSkill emojiPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.emojiPoopSellPrice, () => PlayerData.Instance.EmojiPoopSellLevel, () => PlayerData.Instance.EmojiPoopSellLevel++);
        public static UpgradeSkill dogPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.dogPoopSellPrice, () => PlayerData.Instance.DogPoopSellLevel, () => PlayerData.Instance.DogPoopSellLevel++);
        public static UpgradeSkill pinkPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.pinkPoopSellPrice, () => PlayerData.Instance.PinkPoopSellLevel, () => PlayerData.Instance.PinkPoopSellLevel++);
        public static UpgradeSkill iceCreamPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.iceCreamPoopSellPrice, () => PlayerData.Instance.IceCreamPoopSellLevel, () => PlayerData.Instance.IceCreamPoopSellLevel++);
        public static UpgradeSkill donutPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.donutPoopSellPrice, () => PlayerData.Instance.DonutPoopSellLevel, () => PlayerData.Instance.DonutPoopSellLevel++);
        public static UpgradeSkill rainbowPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.rainbowPoopSellPrice, () => PlayerData.Instance.RainbowPoopSellLevel, () => PlayerData.Instance.RainbowPoopSellLevel++);
        public static UpgradeSkill goldPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.goldPoopSellPrice, () => PlayerData.Instance.GoldPoopSellLevel, () => PlayerData.Instance.GoldPoopSellLevel++);
        public static UpgradeSkill diamondPoopSellPrice = new UpgradeSkill(UpgradableSkillFactory.diamondPoopSellPrice, () => PlayerData.Instance.DiamondPoopSellLevel, () => PlayerData.Instance.DiamondPoopSellLevel++);
    }
}