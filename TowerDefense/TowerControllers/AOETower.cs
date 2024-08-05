using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : TowerBase
{
    [SerializeField] private ParticleSystem _flameParticle;
    [SerializeField] private TowerRange _coneRange;
    [SerializeField] private List<EnemyController> _deadEnemies = new List<EnemyController>();
    private List<EnemyController> _enemiesInConeRange = new List<EnemyController>();

    protected override void Start()
    {
        base.Start();
        _enemiesInConeRange = _coneRange.GetListReference();
    }
    protected override void Update(){
        if(GetFirstTargetInRange() != null)
            LookAtTarget();

        // remove dead targets here
        RemoveDeadsFromLists();

        if(CanShoot()){
            Shoot();
        }
            
    }

    protected override void Shoot()
    {
        foreach(EnemyController enemyController in _targetsInRange)
            if(!enemyController.GetIsAlive())
                _deadEnemies.Add(enemyController);

        RemoveDeadsFromLists();

        foreach(EnemyController enemyController in _targetsInRange){
            if(!enemyController.GetIsAlive()){
                continue;
            }
                

            if(_enemiesInConeRange.Contains(enemyController))
                enemyController.TakeDamage(_shotDamage);
                
        } 
        
        _towerAnimationController.PlayShootingAnimation();
        _flameParticle.Play();
    }

    protected override bool CanShoot()
    {   
        return base.CanShoot() && GetHasAliveEnemyInRange() && GetHasAliveEnemyInConeRange();
    }

    private bool GetHasAliveEnemyInRange(){
        foreach(EnemyController enemyController in _targetsInRange){
            if(enemyController.GetIsAlive())
                return true;
        }
        return false;
    }

    private bool GetHasAliveEnemyInConeRange(){
        foreach(EnemyController enemyController in _enemiesInConeRange){
            if(enemyController.GetIsAlive())
                return true;
        }

        return false;
    }

    private void RemoveDeadsFromLists(){
        foreach(EnemyController enemy in _deadEnemies){
            if(_targetsInRange.Contains(enemy))
                _targetsInRange.Remove(enemy);
            if(_enemiesInConeRange.Contains(enemy))
                _enemiesInConeRange.Remove(enemy);
        }

        _deadEnemies.Clear();
    }

}
