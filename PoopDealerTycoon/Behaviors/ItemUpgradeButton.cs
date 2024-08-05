using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class ItemUpgradeButton : BaseUpgradeButton
    {
        [SerializeField] private TextMeshProUGUI _pricePercentageText;

        protected override void UpdateUIElements()
        {
            base.UpdateUIElements();
            _pricePercentageText.text = ((int)_upgradeSkill.GetCurrentLevelCoef() * 100 + "%");
        }
    }
}