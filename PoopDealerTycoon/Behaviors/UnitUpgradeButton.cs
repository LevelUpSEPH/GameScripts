using UnityEngine;
using TMPro;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class UnitUpgradeButton : BaseUpgradeButton
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        protected override void EnableMaxUI()
        {
            base.EnableMaxUI();
            _levelText.text = "MAX";
        }

        protected override void UpdateNormalUI()
        {
            base.UpdateNormalUI();
            _levelText.text = "Lv." + _upgradeSkill.LevelInData();
        }
           
    }
}