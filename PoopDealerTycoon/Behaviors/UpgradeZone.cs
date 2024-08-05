using UnityEngine;
using Game.Scripts.Models;
using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class UpgradeZone : MonoBehaviour
    {
        public static event Action<Transform> UpgradeZoneFirstEnable;
        [SerializeField] private GameObject _conesParent;
        [SerializeField] private GameObject _visuals;

        private void Start()
        {
            if(PlayerData.Instance.IsPermanentCheckoutActive)
                SetZoneEnabled(true);
            else
            {
                SetZoneEnabled(false);
                PlayerData.Stats[StatKeys.IsPermanentCheckoutActive].Changed += OnPermanentCheckoutActived;
            }
        }

        private void OnDisable()
        {
            PlayerData.Stats[StatKeys.IsPermanentCheckoutActive].Changed -= OnPermanentCheckoutActived;
        }

        private void OnPermanentCheckoutActived()
        {
            SetZoneEnabled(true);
        }

        private void SetZoneEnabled(bool isZoneEnabled)
        {
            _conesParent.SetActive(!isZoneEnabled);
            _visuals.SetActive(isZoneEnabled);
            if(isZoneEnabled)
            {
                if(PlayerData.Instance.PlayerSawUpgradeZone)
                    return;
                UpgradeZoneFirstEnable?.Invoke(transform);
                PlayerData.Instance.PlayerSawUpgradeZone = true;
            }
        }
    }
}