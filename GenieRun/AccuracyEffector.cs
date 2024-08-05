using System.Collections;
using UnityEngine;
using System;

public class AccuracyEffector : MonoBehaviour // On AccuracyEffectors
{
    [SerializeField] private int points;
    [SerializeField] private GameObject _model;
    [SerializeField] private ParticleSystem _particle;

    private bool _isTriggeredBefore = false;
    /*[SerializeField] private GameObject _collectableParticlePrefab;

    private void CollectedAccuracyEffector(){
        StartCoroutine(DestroyWithDelay());
        Instantiate(_collectableParticlePrefab, transform.position, Quaternion.Euler(-90, 0, 0));
    }

    private IEnumerator DestroyWithDelay(){
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }*/

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if (_isTriggeredBefore)
                return;
            _isTriggeredBefore = true;
            other.gameObject.GetComponent<PlayerController>().ChangeAccuracyExp(points);
            _particle.Play();
            _model.SetActive(false);
        }
    }
}
