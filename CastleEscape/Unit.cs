using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    public static event Action<Unit> AnyUnitDied;
    [SerializeField] protected int _unitLevel = 0;
    [SerializeField] private GameObject _deathParticle;
    protected Animator _unitAnimator;
    private float _attackCooldown = 0.5f;
    protected bool _isAttacking = false;

    public int GetLevel(){
        return _unitLevel;
    }

    public void IncrementLevel(int amount){
        _unitLevel += amount;
    }


    protected virtual bool TagCheck(Collider other){
        return false;
    }

    protected virtual void HandleFight(Collider other){
        Unit targetUnit = other.gameObject.GetComponent<Unit>();
            
            if(_unitLevel >= targetUnit.GetLevel() && !_isAttacking){
                _isAttacking = true;
                PlayAttackAnimation();
                StartCoroutine(HandleAttackCooldown());
            }
            
            else if(_unitLevel < targetUnit.GetLevel())
            {
                Die();
            }
    }

    protected IEnumerator DieWithDelay(){
        _unitAnimator.SetTrigger("Die");
        float deathDelay = 1f;
        yield return new WaitForSeconds(deathDelay);
        Vector3 instantiatePosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Instantiate(_deathParticle, instantiatePosition, _deathParticle.transform.rotation);
        Destroy(gameObject);
        AnyUnitDied?.Invoke(this);
    }

    public virtual void Die(){
        StartCoroutine(DieWithDelay());
    }

    protected void PlayAttackAnimation(){
        _unitAnimator.SetTrigger("Attacking");
    }

    public void PlayMoveAnimation(){
        _unitAnimator.SetBool("Walking", true);
    }

    public void StopMoveAnimation(){
        _unitAnimator.SetBool("Walking", false);
    }

    public void PlayFearAnimation(){
        _unitAnimator.SetBool("Fear", true);
    }

    protected IEnumerator HandleAttackCooldown(){
        yield return new WaitForSeconds(_attackCooldown);
        _isAttacking = false;
    }

}
