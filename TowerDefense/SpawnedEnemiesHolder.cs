using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedEnemiesHolder : Singleton<SpawnedEnemiesHolder>
{
    private List<EnemyController> _spawnedEnemies = new List<EnemyController>();

    // spawned enemies add themselves to the list and remove themselves on death

    public List<EnemyController> GetSpawnedEnemyList(){
        return _spawnedEnemies;
    }

    public void AddToSpawnedEnemies(EnemyController enemy){
        _spawnedEnemies.Add(enemy);
    }

    public void RemoveFromSpawnedEnemies(EnemyController enemy){
        _spawnedEnemies.Remove(enemy);
    }
}
