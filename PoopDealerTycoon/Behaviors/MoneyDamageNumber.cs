using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class MoneyDamageNumber : MonoBehaviour
    {
        [SerializeField] private DamageNumberMesh _damageNumberPrefab;
        [SerializeField] private Transform _playerTransform;

        void Start()
        {
            MoneyStackingArea.MoneyCollected += OnCollectedMoney;
        }

        private void OnDisable(){
            MoneyStackingArea.MoneyCollected -= OnCollectedMoney;
        }

        private void OnCollectedMoney(int amount){
            Vector3 spawnPos = _playerTransform.position + Vector3.up * 1.8f;
            DamageNumber damageNumber = _damageNumberPrefab.Spawn(spawnPos, _playerTransform);
            damageNumber.number = amount;
        }

    }
}