using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InLevelMoneyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start(){
        UpdateMoneyAmount();
        MoneyController.MoneyAmountChanged += OnMoneyAmountChanged;
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable(){
        MoneyController.MoneyAmountChanged -= OnMoneyAmountChanged;
        LevelBehaviour.Started -= OnLevelStarted;
    }

    private void OnMoneyAmountChanged(){
        UpdateMoneyAmount();
    }

    private void OnLevelStarted(){
        UpdateMoneyAmount();
    }

    public void UpdateMoneyAmount(){
        _moneyText.text = MoneyController.instance.GetMoney().ToString();
    }

}
