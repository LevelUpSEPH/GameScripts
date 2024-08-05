using System.Collections;
using System.Collections.Generic;
using System;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class UpgradeSkill
    {
        public List<UpgradableSkill> upgradableSkill;
        public event Action UpgradeEvent;
        public Func<int> LevelInData;
        public Action IncrementLevel;

        public UpgradeSkill(List<UpgradableSkill> upgradableSkill, Func<int> LevelInData, Action IncrementLevel)
        {
            this.upgradableSkill = upgradableSkill;
            this.LevelInData = LevelInData;
            this.IncrementLevel = IncrementLevel;
        }

        public void InvokeUpgradeEvent()
        {
            UpgradeEvent?.Invoke();
        }

        public int GetUpgradeCost()
        {
            return upgradableSkill[LevelInData()].price;
        }

        public float GetCurrentLevelCoef()
        {
            return upgradableSkill[LevelInData() - 1].coef;
        }

        public bool GetIsMax()
        {
            return upgradableSkill.Count <= LevelInData();
        }

        public class UpgradableSkill {
            public int price;
            public float coef;

            public UpgradableSkill(int price, float coef)
            {
                this.price = price;
                this.coef = coef;
            }
        }
    }
}
