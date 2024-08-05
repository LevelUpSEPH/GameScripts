/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    public enum DoorType{
        LoveDoor,
        MoneyDoor,
        AdDoor
    }

    [SerializeField] private GameObject _doorModel;
    [SerializeField] private DoorType _doorType = DoorType.LoveDoor;

    public static event Action<DoorType> DoorTriggered;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            DoorTriggered?.Invoke(_doorType);
            GoTransparent();
        }
            
    }

    private void GoTransparent(){
        Renderer doorMat = _doorModel.GetComponent<Renderer>();
        doorMat.material.color = new Color(doorMat.material.color.r,doorMat.material.color.g, doorMat.material.color.b,0);
    }

}
*/