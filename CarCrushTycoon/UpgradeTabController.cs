using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle
{
    public class UpgradeTabController : MonoBehaviour
    {
        [SerializeField] private UpgradeElement _restockSpeedUpgradeElement;
        [SerializeField] private UpgradeElement _incomeUpgradeElement;
        [SerializeField] private UpgradeElement _personelSpeedUpgradeElement;

        private void OnEnable()
        {
            RegisterEvents();

            UpdateUI();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            _restockSpeedUpgradeElement.buyUpgradeButton.onClick.AddListener(UpgradeRestockSpeedLevel);
            _incomeUpgradeElement.buyUpgradeButton.onClick.AddListener(UpgradeIncomeLevel);
            _personelSpeedUpgradeElement.buyUpgradeButton.onClick.AddListener(UpgradePersonelSpeedLevel);
        }

        private void UnregisterEvents()
        {
            _restockSpeedUpgradeElement.buyUpgradeButton.onClick.RemoveListener(UpgradeRestockSpeedLevel);
            _incomeUpgradeElement.buyUpgradeButton.onClick.RemoveListener(UpgradeIncomeLevel);
            _personelSpeedUpgradeElement.buyUpgradeButton.onClick.RemoveListener(UpgradePersonelSpeedLevel);
        }

        private void UpgradeRestockSpeedLevel()
        {
            if(UpgradeController.instance.TryIncreaseRestockSpeedLevel())
                UpdateUI();
        }

        private void UpgradeIncomeLevel()
        {
            if(UpgradeController.instance.TryIncreaseIncomeLevel())
                UpdateUI();
        }

        private void UpgradePersonelSpeedLevel()
        {
            if(UpgradeController.instance.TryIncreasePersonelSpeedLevel())
                UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateRestockElement();
            UpdateIncomeElement();
            UpdatePersonelSpeedElement();
        }

        private void UpdateRestockElement()
        {
            int buyPrice = 0;
            float nextCoef = 0;
            bool isUpgradeMax = UpgradeController.instance.IsRestockSpeedLevelMax();
            if(!isUpgradeMax)
            {
                buyPrice = UpgradeController.instance.restockSpeed[PlayerData.Instance.RestockSpeedLevel - 1].price;
                nextCoef = 1 + (1 - UpgradeController.instance.restockSpeed[PlayerData.Instance.RestockSpeedLevel].coef);
            }
            
            UpdateUpgradeElement(_restockSpeedUpgradeElement, isUpgradeMax, 1 + (1 - UpgradeController.instance.restockSpeed[PlayerData.Instance.RestockSpeedLevel - 1].coef),nextCoef, buyPrice);
        }

        private void UpdateIncomeElement()
        {
            int buyPrice = 0;
            float nextCoef = 0;
            bool isUpgradeMax = UpgradeController.instance.IsIncomeLevelMax();
            if(!isUpgradeMax)
            {
                buyPrice = UpgradeController.instance.income[PlayerData.Instance.IncomeLevel - 1].price;
                nextCoef = UpgradeController.instance.income[PlayerData.Instance.IncomeLevel].coef;
            }
            
            UpdateUpgradeElement(_incomeUpgradeElement, isUpgradeMax, UpgradeController.instance.income[PlayerData.Instance.IncomeLevel - 1].coef,nextCoef, buyPrice);
        }

        private void UpdatePersonelSpeedElement()
        {
            int buyPrice = 0;
            float nextCoef = 0;
            bool isUpgradeMax = UpgradeController.instance.IsPersonelSpeedLevelMax();
            if(!isUpgradeMax)
            {
                buyPrice = UpgradeController.instance.personelSpeed[PlayerData.Instance.PersonelSpeedLevel - 1].price;
                nextCoef = UpgradeController.instance.personelSpeed[PlayerData.Instance.PersonelSpeedLevel].coef;
            }
            
            UpdateUpgradeElement(_personelSpeedUpgradeElement, isUpgradeMax, UpgradeController.instance.personelSpeed[PlayerData.Instance.PersonelSpeedLevel - 1].coef,nextCoef, buyPrice);
        }

        private void UpdateUpgradeElement(UpgradeElement elementToUpdate, bool isLevelMax, float currentCoef, float nextCoef = 0, int buyPrice = 0)
        {
            if(isLevelMax)
            {
                elementToUpdate.SwitchToMaxGroup();
                elementToUpdate.upgradeCoefChangeText.text = currentCoef.ToString("f2") + "x";
            }
            else
            {
                elementToUpdate.SwitchToBuyableGroup();

                elementToUpdate.buyUpgradeButton.interactable = buyPrice <= PlayerData.Instance.CurrencyAmount;
                elementToUpdate.SetBuyPriceText(buyPrice);

                elementToUpdate.upgradeCoefChangeText.text = currentCoef.ToString("f2") + "x" + " -> " + nextCoef.ToString("f2") + "x";
            }
        }
    }

    [System.Serializable]
    public class UpgradeElement
    {
        public Button buyUpgradeButton;
        public TextMeshProUGUI upgradePriceText;
        public TextMeshProUGUI upgradeCoefChangeText;
        public GameObject maxGroup;
        public GameObject buyableGroup;

        public void SwitchToMaxGroup()
        {
            buyUpgradeButton.interactable = false;

            maxGroup.SetActive(true);
            buyableGroup.SetActive(false);
        }

        public void SwitchToBuyableGroup()
        {
            maxGroup.SetActive(false);
            buyableGroup.SetActive(true);
        }

        public void SetBuyPriceText(int buyPrice)
        {
            upgradePriceText.text = buyPrice.ToString();
        }

    }
}