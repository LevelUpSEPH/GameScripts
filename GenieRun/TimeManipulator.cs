using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulator : MonoBehaviour
{

    private void Awake() {
        SelectionDoor.SelectionDoorTriggered += SetTimeScaleToDefault;
    }

    private void OnDisable() {
        SelectionDoor.SelectionDoorTriggered -= SetTimeScaleToDefault;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Time Scale Decreased");
            Time.timeScale = 0.3f;
        }
    }

    private void SetTimeScaleToDefault() {
        Time.timeScale = 1;
    }
}
