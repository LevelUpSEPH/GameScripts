using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Money")){
            MoneyBehaviour MoneyBehaviour = other.GetComponent<MoneyBehaviour>();
            MoneyBehaviour.Collect(transform);
        }
    }


}
