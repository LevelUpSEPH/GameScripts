using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData = Game.Scripts.Models.PlayerData;

namespace Chameleon.Game.ArcadeIdle
{
    public class StaffHireController : Singleton<StaffHireController>
    {
        [SerializeField] private List<Staff> _staffByIndex = new List<Staff>();

        protected override void Awake()
        {
            base.Awake();

            InitializeActiveStaff();
        }

        private void InitializeActiveStaff()
        {
            for(int i = 0; i <= PlayerData.Instance.LastHiredStaffIndex; i++)
            {
                ActivateStaff(i);
            }
        }

        public void ActivateStaff(int staffIndex)
        {
            if(staffIndex > PlayerData.Instance.LastHiredStaffIndex)
            {
                PlayerData.Instance.LastHiredStaffIndex = staffIndex;
            }

            _staffByIndex[staffIndex].staffObject.SetActive(true);
        }
        
        public bool GetIsStaffOwned(int targetIndex)
        {
            return PlayerData.Instance.LastHiredStaffIndex >= targetIndex;
        }

        public int GetPriceOfStaffByIndex(int staffIndex)
        {
            return _staffByIndex[staffIndex].price;
        }

        public bool GetIsStaffLocked(int targetIndex)
        {
            return targetIndex > PlayerData.Instance.LastHiredStaffIndex + 1;
        }
    }

    [System.Serializable]
    public struct Staff
    {
        public GameObject staffObject;
        public int price;
    }
}