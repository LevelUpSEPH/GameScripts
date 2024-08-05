using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerData = Game.Scripts.Models.PlayerData;

namespace Chameleon.Game.ArcadeIdle
{
    public class HireTabController : MonoBehaviour // hire controller or someone of the kind will check the staff prices/bought status
    {
        [SerializeField] private List<StaffElements> _staff = new List<StaffElements>();

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
            _staff[0].buyStaffButton.onClick.AddListener(BuyFirstStaff);
            /*_staff[1].buyStaffButton.onClick.AddListener(BuySecondStaff);
            _staff[2].buyStaffButton.onClick.AddListener(BuyThirdStaff);
            _staff[3].buyStaffButton.onClick.AddListener(BuyFourthStaff);
            _staff[4].buyStaffButton.onClick.AddListener(BuyFifthStaff);*/
        }

        private void UnregisterEvents()
        {
            _staff[0].buyStaffButton.onClick.RemoveListener(BuyFirstStaff);
            /*_staff[1].buyStaffButton.onClick.RemoveListener(BuySecondStaff);
            _staff[2].buyStaffButton.onClick.RemoveListener(BuyThirdStaff);
            _staff[3].buyStaffButton.onClick.RemoveListener(BuyFourthStaff);
            _staff[4].buyStaffButton.onClick.RemoveListener(BuyFifthStaff);*/
        }

        private void UpdateUI()
        {
            for(int i = 0; i < _staff.Count; i++)
            {
                UpdateUIElementByIndex(i);
            }
        }

        private void UpdateUIElementByIndex(int elementIndex)
        {
            if(StaffHireController.instance.GetIsStaffOwned(elementIndex))
            {
                _staff[elementIndex].SwitchToOwnedGroup();
            }
            else if(StaffHireController.instance.GetIsStaffLocked(elementIndex))
            {
                _staff[elementIndex].SwitchToLockedGroup();
            }
            else
            {
                _staff[elementIndex].SwitchToBuyableGroup();
                int buyPrice = StaffHireController.instance.GetPriceOfStaffByIndex(elementIndex);

                if(PlayerData.Instance.CurrencyAmount >= buyPrice)
                {
                    _staff[elementIndex].buyStaffButton.interactable = true;
                }
                else
                {
                    _staff[elementIndex].buyStaffButton.interactable = false;
                }

                _staff[elementIndex].SetBuyPrice(buyPrice);
            }
        }

        private void BuyFirstStaff()
        {
            BuyStaffWithIndex(0);
        }

        private void BuySecondStaff()
        {
            BuyStaffWithIndex(1);
        }

        private void BuyThirdStaff()
        {
            BuyStaffWithIndex(2);
        }

        private void BuyFourthStaff()
        {
            BuyStaffWithIndex(3);
        }

        private void BuyFifthStaff()
        {
            BuyStaffWithIndex(4);
        }

        private void BuyStaffWithIndex(int targetStaffIndex)
        {
            int buyPrice = StaffHireController.instance.GetPriceOfStaffByIndex(targetStaffIndex);
            if(PlayerData.Instance.CurrencyAmount >= buyPrice)
            {
                PlayerData.Instance.PayCurrency(buyPrice);
                StaffHireController.instance.ActivateStaff(targetStaffIndex);

                UpdateUI();
            }
        }
    }

    [System.Serializable]
    public class StaffElements
    {
        public Button buyStaffButton;
        public TextMeshProUGUI buyPrice;
        public GameObject staffBuyableGroup;
        public GameObject staffOwnedGroup;
        public GameObject staffLockedGroup;

        public void SwitchToBuyableGroup()
        {
            staffBuyableGroup.SetActive(true);
            staffOwnedGroup.SetActive(false);
            staffLockedGroup.SetActive(false);
        }

        public void SwitchToOwnedGroup()
        {
            buyStaffButton.interactable = false;

            staffBuyableGroup.SetActive(false);
            staffOwnedGroup.SetActive(true);
            staffLockedGroup.SetActive(false);
        }

        public void SwitchToLockedGroup()
        {
            buyStaffButton.interactable = false;

            staffBuyableGroup.SetActive(false);
            staffOwnedGroup.SetActive(false);
            staffLockedGroup.SetActive(true);
        }

        public void SetBuyPrice(int priceToSet)
        {
            buyPrice.text = priceToSet.ToString();
        }
    }
}