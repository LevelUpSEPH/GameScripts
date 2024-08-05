using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;

    public void PlayWalkingAnimation(){
        _enemyAnimator.SetBool("Walking", true);
    }

    public void PlayRunningAnimation(){
        _enemyAnimator.SetBool("Running", true);
    }

}
