using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    public void PlayMoveAnimation(){
        _playerAnimator.SetBool("Walking", true);
    }

    public void StopMoveAnimation(){
        _playerAnimator.SetBool("Walking", false);
    }

    public void PlayShootingAnimation(){
        _playerAnimator.SetBool("Shooting", true);
    }

    public void StopShootingAnimation(){
        _playerAnimator.SetBool("Shooting", false);
    }

    public void SetAnimationSpeed(float animationSpeed){
        _playerAnimator.speed = animationSpeed;
    }

    // other animations here :
}
