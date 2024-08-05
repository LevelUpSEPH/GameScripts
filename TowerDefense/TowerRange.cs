using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    [SerializeField] protected List<EnemyController> _targetsInRange = new List<EnemyController>();

    private void Start(){
        EnemyController.EnemyDied += OnEnemyDied;
    }

    private void OnDisable(){
        EnemyController.EnemyDied -= OnEnemyDied;
    }
    
    public void UpdateSphereCollider(float radius){
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Enemy")){
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if(!_targetsInRange.Contains(enemyController))
                _targetsInRange.Add(enemyController);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Enemy")){
            _targetsInRange.Remove(other.gameObject.GetComponent<EnemyController>());
        }
    }

    private void OnEnemyDied(EnemyController deadEnemy){
        if(_targetsInRange.Contains(deadEnemy))
            _targetsInRange.Remove(deadEnemy);
    }

    public List<EnemyController> GetListReference(){
        return _targetsInRange;
    }

}
