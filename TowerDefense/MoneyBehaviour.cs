using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class MoneyBehaviour : MonoBehaviour
{
    public static event Action<int> CollectedMoney;
    
    [SerializeField] int _moneyValue = 5;

    [SerializeField] private float _flyingSpeed = 20f;
    private Transform _collectorTransform = null;
    private Tweener _movementTween = null;
    private bool _beingCollected = false;

    private void OnEnable(){
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable(){
        LevelBehaviour.Started -= OnLevelStarted;

        ResetMoney();
    }

    private void Update(){
        
        if(_collectorTransform == null)
            return;
        if(Vector3.Distance(transform.position, _collectorTransform.position) < 1f){
            CompleteCollect();
        }
        if(_movementTween != null){
            _movementTween.ChangeEndValue(_collectorTransform.position,-1, true);
        }
        
    }

    public void JumpTowards(Vector3 position){
        transform.DOJump(position, 0.2f, 1, 0.7f);
    }

    public void Collect(Transform collectorTransform){
        if(_beingCollected)
            return;
        transform.DOKill();
        _collectorTransform = collectorTransform;
        _movementTween = transform.DOMove(collectorTransform.position, _flyingSpeed).SetSpeedBased(true);
        _beingCollected = true;
    }

    private void CompleteCollect(){
        CollectedMoney?.Invoke(_moneyValue);
        gameObject.SetActive(false);
    }

    private void ResetMoney(){
        transform.DOKill();
        _collectorTransform = null;
        _movementTween = null;
        _beingCollected = false;
    }

    private void OnLevelStarted(){
        gameObject.SetActive(false);
    }

}
