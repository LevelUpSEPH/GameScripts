using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField] private Animator _playerAnim;
    [SerializeField] private GameObject _playerModel;
    public static event Action StartAnimationEnded;

    private void OnEnable(){
        InLevelController.LevelStarted += OnLevelStarted;
    }

    private void OnDisable(){
        InLevelController.LevelStarted -= OnLevelStarted;
    }

    private void OnLevelStarted(){
        HandleRiseAtBeginning();
        _playerModel.transform.DORotate(new Vector3(0,0,0), 0.5f,RotateMode.FastBeyond360)
        .OnComplete(() => StartAnimationEnded?.Invoke());
        
    }

    private void HandleRiseAtBeginning() {
        transform.DOMoveY(0, 0.5f);
    }

    public void PlayLevelUpAnimation(){
        
        _playerAnim.SetTrigger("Celebrate");
        _playerModel.transform.DORotate(new Vector3(0,360,0),0.25f,RotateMode.FastBeyond360);
    }

    public void PlayWalkAnimation(){
        _playerAnim.SetBool("Running", true);
    }

    public void StopWalkAnimation(){
        _playerAnim.SetBool("Running", false);
    }

}