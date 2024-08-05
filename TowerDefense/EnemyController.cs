using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public static event Action<EnemyController> EnemyDied;
    [SerializeField] private EnemySO _enemyScriptableObject;

    [SerializeField] private GameObject _enemyModel;
    [SerializeField] private GameObject _enemyRagdoll;
    [SerializeField] private EnemyHealthBarUI _enemyHealthbar;
    
    private int _maxHealth;
    private float _movementSpeed;
    private float _damage;
    private int _littleMoneyReward = 3;
    private int _bigMoneyReward = 3;
    private EnemyMovementController _enemyMovementController;
    private EnemyAnimationController _enemyAnimationController;
    private float _health;
    private bool _isAlive = true;

    private void Awake(){
        InitializeEnemy();

        _enemyMovementController = GetComponent<EnemyMovementController>();
        _enemyAnimationController = GetComponent<EnemyAnimationController>();
        _enemyMovementController.SetMovementSpeed(_movementSpeed);
    }

    private void OnEnable(){
        InLevelController.PlayerDied += OnPlayerDied;
        
        ResetEnemy();
        HandleAnimation();
        SpawnedEnemiesHolder.instance.AddToSpawnedEnemies(this);
        _health = _maxHealth;

    }

    private void OnDisable(){
        InLevelController.PlayerDied -= OnPlayerDied;
        StopAllCoroutines();
    }

    private void InitializeEnemy(){
        _maxHealth = _enemyScriptableObject.maxHealth;
        _movementSpeed = _enemyScriptableObject.movementSpeed;
        _damage = _enemyScriptableObject.damage;
        _littleMoneyReward = _enemyScriptableObject.littleMoneyReward;
        _bigMoneyReward = _enemyScriptableObject.bigMoneyReward;
    }

    private void HandleAnimation(){
        if(_movementSpeed > 5f)
            _enemyAnimationController.PlayRunningAnimation();
        else
            _enemyAnimationController.PlayWalkingAnimation();
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("MainCastle")){
            MainCastle mainCastle = other.GetComponent<MainCastle>();
            mainCastle.TakeDamage(_damage);
            Die(true);
        }
    }


    public void TakeDamage(float damage){
        if(!_isAlive)
            return;
        _health -= damage;
        if(_health <= 0){
            Die();
        }

        _enemyHealthbar.UpdateVisuals();

    }
    
    private void Die(bool diedToCastle = false){
        _isAlive = false;
        SpawnedEnemiesHolder.instance.RemoveFromSpawnedEnemies(this);
        SwitchToRagdoll();
        
            
        _enemyMovementController.SetNavIsStopped(true);

        for(int i = 0; i < _littleMoneyReward; i++){
            Vector3 targetPos = new Vector3(UnityEngine.Random.Range(transform.position.x - 1f, transform.position.x + 1f), transform.position.y, UnityEngine.Random.Range(transform.position.z - 1f, transform.position.z + 1f));
            GameObject money = ObjectPool.instance.SpawnFromPool("LittleMoney", transform.position, Quaternion.identity);
            money.GetComponent<MoneyBehaviour>().JumpTowards(targetPos);
        }

        for(int i = 0; i < _bigMoneyReward; i++){
            Vector3 targetPos = new Vector3(UnityEngine.Random.Range(transform.position.x - 1f, transform.position.x + 1f), transform.position.y, UnityEngine.Random.Range(transform.position.z - 1f, transform.position.z + 1f));
            GameObject money = ObjectPool.instance.SpawnFromPool("BigMoney", transform.position, Quaternion.identity);
            money.GetComponent<MoneyBehaviour>().JumpTowards(targetPos);
        }

        EnemyDied?.Invoke(this);

        if(diedToCastle){
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(DisableWithDelay());

    }

    private void ResetEnemy(){
        SwitchToModel();
        _enemyMovementController.SetNavIsStopped(false);
        _isAlive = true;
    }

    private void SwitchToRagdoll(){
        _enemyModel.SetActive(false);
        _enemyRagdoll.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void SwitchToModel(){
        _enemyRagdoll.SetActive(false);
        _enemyModel.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnPlayerDied(){
        Die(true);
    }

    public bool GetIsAlive(){
        return _isAlive;
    }

    public float GetHealthPercentage(){
        return _health / _maxHealth;
    }

    private IEnumerator DisableWithDelay(){
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

}
