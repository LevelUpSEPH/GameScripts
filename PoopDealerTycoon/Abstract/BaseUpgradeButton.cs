using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Models;
using Chameleon.Game.ArcadeIdle.Upgrade;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class BaseUpgradeButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _moneyImage;
        [SerializeField] private Image _priceBackground;
        [SerializeField] private Sprite _enoughMoneyBackground;
        [SerializeField] private Sprite _notEnoughMoneyBackground;
        [SerializeField] private UpgradeSkillName _upgradeType;
        protected UpgradeSkill _upgradeSkill;
        private Button _upgradeButton;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            UpdateUIElements();
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void Initialize()
        {
            _upgradeSkill = Helpers.UpgradeSkillDict.upgradableSkillsDict[_upgradeType];
            _upgradeButton = GetComponent<Button>();
        }

        private void RegisterEvents()
        {
            PlayerData.Stats[StatKeys.CurrencyAmount].Changed += UpdateUIElements;
            _upgradeSkill.UpgradeEvent += UpdateUIElements;
            _upgradeButton.onClick.AddListener(UpgradeTargetSkill);
        }

        private void UnregisterEvents()
        {
            PlayerData.Stats[StatKeys.CurrencyAmount].Changed -= UpdateUIElements;
            _upgradeSkill.UpgradeEvent -= UpdateUIElements;
            _upgradeButton.onClick.RemoveListener(UpgradeTargetSkill);
        }

        private void UpgradeTargetSkill()
        {
            UpgradeController.TryUpgradeLevel(_upgradeType);
        }

        protected virtual void UpdateUIElements()
        {
            if(_upgradeSkill.GetIsMax())
            {
                EnableMaxUI();
            }
            else
            {
                UpdateNormalUI();
            }
        }

        protected virtual void EnableMaxUI()
        {
            SetButtonAvailable(false);
            SetMoneyImageActive(false);
            _priceText.text = "MAX";
        }

        protected virtual void UpdateNormalUI()
        {
            SetMoneyImageActive(true);
            int upgradeCost = _upgradeSkill.GetUpgradeCost();
            SetButtonAvailable(upgradeCost <= PlayerData.Instance.CurrencyAmount);
            _priceText.text = upgradeCost.ToString();
        }

        private void SetButtonAvailable(bool isButtonAvailable)
        {
            SetHasEnoughMoneyBackgroundEnabled(isButtonAvailable);
            _upgradeButton.interactable = isButtonAvailable;
        }

        private void SetHasEnoughMoneyBackgroundEnabled(bool hasEnoughMoney)
        {
            if(hasEnoughMoney)
                _priceBackground.sprite = _enoughMoneyBackground;
            else
                _priceBackground.sprite = _notEnoughMoneyBackground;
        }

        private void SetMoneyImageActive(bool isMoneyImageActive)
        {
            _moneyImage.gameObject.SetActive(isMoneyImageActive);
        }
    }
}