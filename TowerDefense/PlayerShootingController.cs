using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour // is some kind of tower actually? 
{
    [SerializeField] private TowerRange _playerRange;
    [SerializeField] private Transform _shootingOriginTransform;

    [SerializeField] private float _baseShootingCD = 1f;
    [SerializeField] private float _baseAttackDamage = 5f;
    [SerializeField] private float _baseAttackRange = 5f;
    private float _shootingCD;
    private float _attackDamage;
    private List<EnemyController> _targetsInRange = new List<EnemyController>();
    private EnemyController _targetEnemy = null;
    private Transform _playerModelTransform;
    private bool _readyToShoot = true;
    private PlayerAnimationController _playerAnimationController;
    private Vector3 _targetPos;

    private void Start(){
        _targetsInRange = _playerRange.GetListReference();
        _playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        if(_targetEnemy != null){
            LookAtTarget();
        }

        if(CanShoot()){
            GetNearestTarget();
            if(_targetEnemy != null){
                LookAtTarget();
            }
            StartCoroutine(ShootCoroutine());
            
        }

        HandleAnimations();
    }

    private void HandleAnimations(){
        if(_targetEnemy != null)
            _playerAnimationController.PlayShootingAnimation();
        else
            _playerAnimationController.StopShootingAnimation();
    }

    protected IEnumerator ShootCoroutine(){
        _readyToShoot = false;
        yield return new WaitForSeconds(_shootingCD);
        GetNearestTarget();
        if(_targetEnemy != null)
            Shoot();
        _readyToShoot = true;
    }

    private bool CanShoot(){
        if(!_readyToShoot)
            return false;
        if(_targetsInRange.Count <= 0)
            return false;
        
        return true;
    }

    private void Shoot(){
        if(_targetEnemy == null){
            return;
        }
        
        _targetEnemy.TakeDamage(_attackDamage);

        if(!_targetEnemy.GetIsAlive()){
            _targetsInRange.Remove(_targetEnemy);
            _targetEnemy = null;
        }
    
        GameObject shotParticle = ObjectPool.instance.SpawnFromPool("ShotParticle", _shootingOriginTransform.position, _shootingOriginTransform.rotation);
    }

    private void LookAtTarget() {
        _targetPos = new Vector3(_targetEnemy.transform.position.x, transform.position.y, _targetEnemy.transform.position.z);
        _playerModelTransform.LookAt(_targetPos);
    }

    private void GetNearestTarget(){
        List<EnemyController> enemiesInRange = new List<EnemyController>();

        foreach(EnemyController enemy in SpawnedEnemiesHolder.instance.GetSpawnedEnemyList())
            enemiesInRange.Add(enemy);
        float minDistToPlayer = Mathf.Infinity;
        EnemyController nearestEnemy = null;
        for(int i = 0; i < enemiesInRange.Count; i++){

            if(!_targetsInRange.Contains(enemiesInRange[i]) || !enemiesInRange[i].GetIsAlive()){
                continue;
            }

            float distToPlayer = Vector3.Distance(enemiesInRange[i].transform.position, transform.position);
            if(distToPlayer < minDistToPlayer){
                nearestEnemy = enemiesInRange[i];
                minDistToPlayer = distToPlayer;
            }

        }
        _targetEnemy = nearestEnemy;
    }

    public void SetAttackSpeedCoefficient(float shootingCDCoef){
        _shootingCD = _baseShootingCD * shootingCDCoef;
    }

    public void SetAttackDamageCoefficient(float attackDamageCoef){
        _attackDamage = attackDamageCoef * _baseAttackDamage;
    }

    public void SetAttackRangeCoefficient(float attackRangeCoef){
        _playerRange.UpdateSphereCollider(_baseAttackRange * attackRangeCoef);
    }

    public void SetPlayerModelTransform(Transform playerModelTransform){
        _playerModelTransform = playerModelTransform;
    }

    public void SetBaseAttackRange(float attackRange){
        _baseAttackRange = attackRange;
    }

    public bool GetHasTargetInRange(){
        return _targetEnemy != null;
    }

}
