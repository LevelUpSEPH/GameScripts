using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationController : MonoBehaviour
{
    private Animator _towerAnimator;

    public void SetActiveAnimator(Animator animator){
        _towerAnimator = animator;
    }

    public void PlayShootingAnimation(){
        _towerAnimator.SetTrigger("Shooting");
    }
    
    public void StopShootingAnimation(){
        _towerAnimator.SetTrigger("StopAnimation");
    }
    
}
