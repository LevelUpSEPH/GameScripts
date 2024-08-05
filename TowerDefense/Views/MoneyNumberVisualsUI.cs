using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class MoneyNumberVisualsUI : MonoBehaviour
{
    [SerializeField] private DamageNumberGUI _moneyNumberPrefab;
    [SerializeField] private RectTransform _uiRectTransform;

    private void Start(){
        WaveController.WaveEnded += OnWaveEnded;
    }

    private void OnDisable(){
        WaveController.WaveEnded -= OnWaveEnded;
    }

    private void OnWaveEnded(int unused){
        DamageNumber damageNumber = _moneyNumberPrefab.Spawn(Vector3.zero);

        //damageNumber.SetAnchoredPosition(_uiRectTransform, new Vector2(Screen.width/2, Screen.height/2));
        damageNumber.SetAnchoredPosition(_uiRectTransform, new Vector2(0, 400));

        damageNumber.number = WaveController.instance.GetWaveReward();
    }

}
