using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinishTrigger : MonoBehaviour
{
    public static event Action FinishTriggered;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.S))
            FinishTriggered?.Invoke();
    }

    private void OnTriggerEnter(Collider other){
        if(!other.CompareTag("Player")){
            return;
        }   
        if(InLevelController.instance.TargetCountLeftToCatch <= 0)
            FinishTriggered?.Invoke();
    }
}
