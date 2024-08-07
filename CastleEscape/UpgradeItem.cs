using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeItem : MonoBehaviour
{
    public static event Action AnyUpgradeReceived;

    [SerializeField] private int _upgradeValue = 1;

    public void TakeUpgrade(Unit unit){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Unit playerUnit = player.GetComponent<Unit>();
        playerUnit.IncrementLevel(_upgradeValue);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            Unit playerUnit = other.gameObject.GetComponent<Unit>();
            TakeUpgrade(playerUnit);
            AnyUpgradeReceived?.Invoke();
        }
    }
}
