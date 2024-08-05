using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCastle : MonoBehaviour
{
    public static event Action CastleHealthChanged;
    public static event Action CastleDied;

    [SerializeField] private float _maxHealth = 100f;
    private float _health;
    private bool _isAlive = true;

    private void Awake(){
        _health = _maxHealth;
    }

    private void Start(){
        CastleHealthChanged?.Invoke();
    }

    public void TakeDamage(float damage){
        _health -= damage;

        if(_health <= 0){
            Die();
            _health = 0;
        }

        CastleHealthChanged?.Invoke();
    }

    private void Die(){
        if(!_isAlive)
            return;
        Debug.Log("Castle died");
        CastleDied?.Invoke();
        _isAlive = false;
    }

    public float GetHealth(){
        return _health;
    }

    public float GetHealthNormalized(){
        return _health / _maxHealth;
    }

    public float GetMaxHealth(){
        return _maxHealth;
    }
}
