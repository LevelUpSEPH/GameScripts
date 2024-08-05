using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class WarningDamageNumber : MonoBehaviour
    {
        [SerializeField] private DamageNumberMesh _damageNumberPrefab;
        [SerializeField] private Transform _playerTransform;

        void Start()
        {
            Units.PlayerUnit.PlayerCouldntCollectFromZone += OnPlayerCouldntCollectFromZone;
        }

        private void OnDisable(){
            Units.PlayerUnit.PlayerCouldntCollectFromZone -= OnPlayerCouldntCollectFromZone;
        }

        private void OnPlayerCouldntCollectFromZone(){
            Vector3 spawnPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 2.5f, _playerTransform.position.z);
            DamageNumber damageNumber = _damageNumberPrefab.Spawn(spawnPos, _playerTransform);
            damageNumber.topText = "!Shelf doesnt exist!";
        }

    }
}