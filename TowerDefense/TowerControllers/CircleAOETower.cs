using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CircleAOETower : LimitedRangeTower
{
    [SerializeField] private float _damageRadius = 3f;
    private List<EnemyController> _enemiesToDamage = new List<EnemyController>();
    
    protected override void Shoot()
    {
        if(_target == null){
            // dont shoot
            return;
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(_target.transform.position, _damageRadius, 512);

        foreach(Collider collider in hitColliders){
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            if(enemyController.GetIsAlive()){
                _enemiesToDamage.Add(enemyController);
            }
        }

        if(!_target.GetIsAlive()){
            _targetsInRange.Remove(_target);
            _target = null;
        }

        _towerAnimationController.PlayShootingAnimation();    
        GameObject shotParticle = ObjectPool.instance.SpawnFromPool("GrenadeParticle", _shootingOriginTransform.position, _shootingOriginTransform.rotation);

        shotParticle.transform.DOMoveX(_target.transform.position.x, 1f).SetEase(Ease.OutQuad);
        shotParticle.transform.DOMoveZ(_target.transform.position.z, 1f).SetEase(Ease.OutQuad);

        shotParticle.transform.DOMoveY(3.5f, 0.25f).SetEase(Ease.OutQuad)
        .OnComplete(() => shotParticle.transform.DOMoveY(_target.transform.position.y, .75f).SetEase(Ease.InQuad)
        .OnComplete(() => ExplodeGrenade(shotParticle)));

    }

    private void ExplodeGrenade(GameObject grenade){
        DamageEnemiesInRange();

        GameObject explosionParticle = ObjectPool.instance.SpawnFromPool("ExplosionParticle", grenade.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        explosionParticle.transform.eulerAngles = new Vector3(-90, 0, 0);

        grenade.SetActive(false);
    }

    private void DamageEnemiesInRange(){
        foreach(EnemyController enemy in _enemiesToDamage)
            enemy.TakeDamage(_shotDamage);
        _enemiesToDamage.Clear();
        ClearDeadEnemies();
    }

    private void ClearDeadEnemies(){
        for(int i = _targetsInRange.Count - 1 ; i >= 0; i--){
            if(!_targetsInRange[i].GetIsAlive()){
                _targetsInRange.RemoveAt(i);
            }
        }
    }

}
