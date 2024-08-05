using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class MoneyNumberVisuals : MonoBehaviour
{
    [SerializeField] private DamageNumberMesh _damageNumberPrefab;
    private Transform _playerTransform;

    void Start()
    {
        LevelBehaviour.Started += OnLevelStarted;
        MoneyBehaviour.CollectedMoney += OnCollectedMoney;
    }

    private void OnDisable(){
        LevelBehaviour.Started -= OnLevelStarted;
        MoneyBehaviour.CollectedMoney -= OnCollectedMoney;
    }

    private void OnLevelStarted(){
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnCollectedMoney(int amount){
        Vector3 spawnPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 2.5f, _playerTransform.position.z);
        DamageNumber damageNumber = _damageNumberPrefab.Spawn(spawnPos, _playerTransform);
        damageNumber.number = amount; // can be changed
    }

}
