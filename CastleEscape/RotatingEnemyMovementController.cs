using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingEnemyMovementController : MonoBehaviour
{
    [Range(2,15)]
    [SerializeField] private float _rotationInterval = 2f;
    private float _rotationAngleY;
    [SerializeField] private float _rotationAngle = 180;
    private void Start(){
        _rotationAngleY = transform.localRotation.eulerAngles.y;
        StartCoroutine(RotateWithIntervalAndDegree());
    }

    private IEnumerator RotateWithIntervalAndDegree(){
        EnemyController enemyController = GetComponent<EnemyController>();
        while(true){
            yield return new WaitForSeconds(_rotationInterval);

            if(!enemyController.GetCanMove())
                break;

            _rotationAngleY += _rotationAngle;
            Vector3 rotationEndValue = new Vector3(transform.rotation.x, _rotationAngleY, transform.rotation.z);
            
            transform.DORotate(rotationEndValue, 1f, RotateMode.Fast);
        }

    }
}
