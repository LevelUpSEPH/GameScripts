using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    public static event Action WaveSpawned;
    
    [SerializeField] private List<WaveSO> _waveList = new List<WaveSO>();
    [SerializeField] private ParticleSystem _portalParticle;
    [SerializeField] private float _spawningInterval = 0.5f;
    private bool _isSpawnAvailable = true;


    private void Start(){
        InLevelController.PlayerDied += OnPlayerDied;
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable(){
        InLevelController.PlayerDied -= OnPlayerDied;
        LevelBehaviour.Started -= OnLevelStarted;
    }

    public void StartSpawnWave(WaveSO wave) {
        Debug.Log("Wave spawning started");
        StartCoroutine(SpawnWave(wave));
    }

    private IEnumerator SpawnWave(WaveSO wave) {
        _portalParticle.Play();
        foreach (string tag in wave.enemyTags) {
            if(!_isSpawnAvailable)
                break;
            ObjectPool.instance.SpawnFromPool(tag, transform.position, transform.rotation);
            yield return new WaitForSeconds(_spawningInterval);
        }
        Debug.Log("Wave Spawn Finished");
        _portalParticle.Stop();
        WaveSpawned?.Invoke();
    }

    private void OnPlayerDied(){
        _isSpawnAvailable = false;
    }

    private void OnLevelStarted(){
        _isSpawnAvailable = true;
    }

    public List<WaveSO> GetWaveList(){
        return _waveList;
    }

}
