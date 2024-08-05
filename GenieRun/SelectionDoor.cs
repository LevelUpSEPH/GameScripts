using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionDoor : MonoBehaviour
{
    public int points;
    //[SerializeField] private GameObject _selectionDoorModel;
    //[SerializeField] private GameObject _uiParent;
    [SerializeField] private List<GameObject> _objectsToHideWhenTriggered;
    public static event Action SelectionDoorTriggered;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerController>().ChangeIndicatorPoint(points);
            SelectionDoorTriggered?.Invoke();
            RemoveVisuals();
        }
    }

    private void RemoveVisuals(){
        foreach (GameObject obj in _objectsToHideWhenTriggered) {
            obj.SetActive(false);
        }
    }

}