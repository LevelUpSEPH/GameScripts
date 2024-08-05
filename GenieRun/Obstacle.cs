using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int _accuracyEffect = 30;

    [SerializeField] GameObject _fullModel;
    [SerializeField] GameObject _brokenParts;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().ChangeAccuracyExp(_accuracyEffect * -1);
            Break();
        }
    }

    private void Break() {
        _fullModel.SetActive(false);
        _brokenParts.SetActive(true);
    }
}
