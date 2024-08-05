using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveController : Singleton<WaveController>
{        
    private List<WaveSO> _waveList = new List<WaveSO>();
    private EnemySpawner _enemySpawner;
    
    private float _waveTimeLeft;
    private int _currentWaveNo = -1;
    private bool _isCorActive = false;
    private int _maxWaveCount;
    private bool _isWaveSpawnFinished = false;

    public static event Action WaveStarted;
    public static event Action<int> WaveEnded;
    public static event Action WaveChanged;

    protected override void Awake(){
        base.Awake();
    }

    private void Start(){
        EnemyController.EnemyDied += OnEnemyDied;
        EnemySpawner.WaveSpawned += OnWaveSpawnFinished;
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable() {
        EnemyController.EnemyDied -= OnEnemyDied;
        EnemySpawner.WaveSpawned -= OnWaveSpawnFinished;
        LevelBehaviour.Started -= OnLevelStarted;
    }

    private void Update(){
        
        if(Input.GetKeyDown(KeyCode.W)){
            StartNextWave();
        }     
    }
    
    public void StartNextWave(){
        Debug.Log("start next wave");
        StopAllCoroutines();
        _isCorActive = false;

        _currentWaveNo++;
        WaveChanged?.Invoke();
        WaveStarted?.Invoke();
        _enemySpawner.StartSpawnWave(_waveList[_currentWaveNo]);
        
        if(_currentWaveNo + 1 >= _maxWaveCount)
            return;
        _waveTimeLeft = _waveList[_currentWaveNo].waveTime;
        StartCoroutine(WaveCountdown());
        _isCorActive = true;
    }

    private IEnumerator WaveCountdown(){
        while(_waveTimeLeft > 0){
            yield return new WaitForSeconds(1f);
            _waveTimeLeft--;
        }

        Debug.Log("wave timed out");
        WaveEnded?.Invoke(_currentWaveNo);
        _isWaveSpawnFinished = false;
    }

    private void OnEnemyDied(EnemyController unused) {
        if (SpawnedEnemiesHolder.instance.GetSpawnedEnemyList().Count <= 0 && _isWaveSpawnFinished) {
            Debug.Log("Enemies Died in wave");
            StopAllCoroutines();
            _isCorActive = false;
            WaveEnded?.Invoke(_currentWaveNo);
            _isWaveSpawnFinished = false;
        }
    }

    private void OnWaveSpawnFinished(){
        _isWaveSpawnFinished = true;
    }

    private void OnLevelStarted(){
        _enemySpawner = InLevelController.instance.GetEnemySpawner();
        
        _waveList = _enemySpawner.GetWaveList();
        _maxWaveCount = _waveList.Count;
        
        _currentWaveNo = -1;
        WaveChanged?.Invoke();
        // get new wave info from loaded level
        _isWaveSpawnFinished = false;
    }

    public int GetWaveCount(){
        return _maxWaveCount;
    }

    public int GetCurrentWave(){
        return _currentWaveNo;
    }

    public int GetWaveReward(){
        return 180 + _currentWaveNo * 30;
    }

    public float GetRemainingTimeOfWave(){
        return _waveTimeLeft;
    }

    public List<string> GetEnemyTags(){
        return _waveList[_currentWaveNo].enemyTags;
    }

    public bool GetIsCoroutineActive(){
        return _isCorActive;
    }

    [Serializable]
    public class WaveInfo {
        public float waveTime;
        public List<string> enemyTags = new List<string>();
    }
}
