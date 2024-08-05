using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Models;
using UnityEngine;
using System;

namespace Chameleon.Game.ArcadeIdle
{
    public class ZoneBehaviour : MonoBehaviour
    {
        public static event Action<Transform> ZoneActivatedFirstTime;
        [SerializeField] private ZoneType _zoneType;
        [SerializeField] private GameObject _conesParent;
        [SerializeField] private Transform _cameraFocusTarget;

        private void OnEnable()
        {
            OnZoneActivated();
        }

        private void OnZoneActivated()
        {
            if(!GetPlayerSawZone())
            {
                InvokeActivatedForFirstTime();
                PlayerData.Instance.PlayerSawZoneOpen++;
            }
            if(_conesParent != null)
                _conesParent.SetActive(false);
        }

        private bool GetPlayerSawZone()
        {
            return PlayerData.Instance.PlayerSawZoneOpen >= (int)_zoneType;
        }

        private void InvokeActivatedForFirstTime()
        {
            if(_zoneType != ZoneType.FirstZone)
                ZoneActivatedFirstTime?.Invoke(_cameraFocusTarget);
        }
    }

    internal enum ZoneType
    {
        Invalid = 0,
        FirstZone,
        SecondZone,
        ThirdZone
    }
}