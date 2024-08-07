using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void PlayFearParticle(){
        _particleSystem.Play();
    }
}
