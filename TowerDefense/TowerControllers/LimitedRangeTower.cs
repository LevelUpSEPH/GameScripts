using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedRangeTower : TowerBase
{
    [SerializeField] protected float _minRange;

    protected override EnemyController GetFirstTargetInRange()
    {
        List<EnemyController> enemiesInRange = new List<EnemyController>();

        foreach(EnemyController enemy in SpawnedEnemiesHolder.instance.GetSpawnedEnemyList())
            enemiesInRange.Add(enemy);

        for(int i = 0; i < enemiesInRange.Count; i++){
            if(_targetsInRange.Contains(enemiesInRange[i]) && enemiesInRange[i].GetIsAlive()){
                // returns the first enemy thats in range
                if(Vector3.Distance(transform.position, enemiesInRange[i].transform.position) > _minRange){
                    _target = enemiesInRange[i];
                    LookAtTarget();
                    return enemiesInRange[i];
                }
                    
            }
        }

        _target = null;
        return null;
    }

    protected bool HasTargetBetweenRanges(){
        for(int i = 0; i < _targetsInRange.Count; i++){
            if(Vector3.Distance(transform.position, _targetsInRange[i].transform.position) > _minRange)
                return true;
        }

        return false;
    }

    protected override bool CanShoot()
    {   
        return base.CanShoot() && HasTargetBetweenRanges();
    }

    public float GetUnavailableZoneRadius(){
        return _minRange;
    }
}
