using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinishTrigger : MonoBehaviour
{
    public static event Action ParkourFinished;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            ParkourFinished?.Invoke();
        }            
    }
}