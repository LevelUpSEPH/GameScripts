using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _levelEndedParticle;

    private Vector3 _particleThrowPosition;

    private void Start(){
        FinishTrigger.FinishTriggered += OnFinishTriggered;
    }

    private void OnDisable(){
        FinishTrigger.FinishTriggered -= OnFinishTriggered;
    }

    private void OnFinishTriggered(){
        ThrowParticle();
    }

    private void ThrowParticle(){
        _levelEndedParticle.Play();
    }

}
