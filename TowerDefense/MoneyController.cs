using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyController : Singleton<MoneyController>
{
    public static event Action MoneyAmountChanged;

    [SerializeField] private int _startingMoney = 600;
    private int _money;

    protected override void Awake(){
        _money = _startingMoney;
    }
    
    private void Start(){
        WaveController.WaveEnded += OnWaveEnded;
    }

    private void OnDisable(){
        WaveController.WaveEnded -= OnWaveEnded;
    }
    
    public bool TryUseMoney(int amount){

        if(GetHasEnoughMoneyFor(amount)){
            _money -= amount;
            MoneyAmountChanged?.Invoke();
            return true;
        }

        else {
            Debug.Log("Not enough money");
            return false;
        }

    }

    private void OnWaveEnded(int endedWave){
        AddMoney(WaveController.instance.GetWaveReward());
    }

    public bool GetHasEnoughMoneyFor(int price){
        return price <= _money;
    }

    public int GetMoney(){
        return _money;
    }

    public void AddMoney(int amount){
        _money += amount;
        MoneyAmountChanged?.Invoke();
    }

    public void ResetMoney(){
        _money = _startingMoney;
    }



}
