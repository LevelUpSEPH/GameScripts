using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private Vector3 _conveyorDirection;
    [SerializeField] private float _conveyorSpeed = 5;
    private Rigidbody _rigidbody;

    private void Awake(){
        _conveyorDirection = -transform.forward;
    }

    private void Update(){
        if(_rigidbody != null)
            _rigidbody.velocity = _conveyorDirection * _conveyorSpeed;
    }

    private void OnTriggerEnter(Collider other){
        _rigidbody = other.gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(){

    }

}
