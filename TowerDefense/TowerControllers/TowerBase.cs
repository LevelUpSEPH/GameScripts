using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerBase : MonoBehaviour
{
    public static event Action<bool> TowerCountChanged;
    public static event Action TowerUpgraded;

    [Header("Tower Main Components")]
    [SerializeField] private TowerSO _towerScriptableObject;
    [SerializeField] private TowerRange _towerRange;
    [SerializeField] protected Transform _shootingOriginTransform;
    [SerializeField] private Transform _modelsParent;
    [SerializeField] private List<GameObject> _towerModels = new List<GameObject>();

    [Header("Tower Visual Components")]
    [SerializeField] private TowerRangeVisualBehaviour _towerRangeVisual;
    [SerializeField] private TowerPlacementVisual _towerPlacementVisual;
    [SerializeField] private PlacementArrowsController _placementArrowsController;
    [SerializeField] private ParticleSystem _placementParticle;
    [SerializeField] private ParticleSystem _upgradeParticle;
    private TowerSO _initialTowerSO;
    private GameObject _currentModel;
    protected TowerAnimationController _towerAnimationController;
    

    #region TowerScriptableObjectElements
    protected string _towerName;
    protected float _shootingCD;
    protected float _shotDamage;
    protected float _range;
    protected bool _readyToShoot = false; // will be ready to shoot when placed
    protected int _towerCost;
    protected int _upgradeLevel = 1;
    protected int _upgradeCost;
    #endregion

    private TileBehaviour _connectedToTile;

    protected List<EnemyController> _targetsInRange = new List<EnemyController>();
    protected EnemyController _target = null;

    private int _spentMoney = 0;

    private void Awake(){
        _initialTowerSO = _towerScriptableObject;
        UpdateTowerStats();
        _towerAnimationController = GetComponent<TowerAnimationController>();
        
        _currentModel = _towerModels[0];

        _towerAnimationController.SetActiveAnimator(_currentModel.GetComponent<Animator>());
    }

    protected virtual void Start()
    {
        LevelBehaviour.Started += OnLevelStarted;

        _towerRange.UpdateSphereCollider(_range);
        //DisplayRange();

        _targetsInRange = _towerRange.GetListReference();

    }

    private void OnEnable(){
        _towerPlacementVisual.SetVisibility(true);
        _placementArrowsController.gameObject.SetActive(true);
        DisplayRange();
    }

    private void OnDisable(){
        LevelBehaviour.Started -= OnLevelStarted;
        _targetsInRange.Clear();
        
    }

    protected virtual void Update()
    {
        if(_target != null)
            LookAtTarget();

        if(CanShoot()){
            GetFirstTargetInRange();
            if(_target != null)
                LookAtTarget();
            StartCoroutine(ShootCoroutine());
            
        }

    }

    protected virtual bool CanShoot(){
        if(!_readyToShoot)
            return false;
        if(_targetsInRange.Count <= 0)
            return false;
        
        return true;
    }

    public void DisplayRange(){
        _towerRangeVisual.ShowSphere(_range);
    }

    public void HideRange(){
        _towerRangeVisual.HideSphere();
    }

    public void Placed(TileBehaviour placedInsideTile){
        _targetsInRange.Clear();
        _connectedToTile = placedInsideTile;
        _readyToShoot = true;
        _spentMoney += _towerCost * 2 / 3;
        _towerPlacementVisual.SetVisibility(false);
        _placementArrowsController.gameObject.SetActive(false);
        HideRange();
        if(_placementParticle != null)
            _placementParticle.Play();
        TowerCountChanged?.Invoke(true);
    }

    public void SellTower(){
        _connectedToTile.RemoveTower();
        _connectedToTile = null;
        _readyToShoot = false;

        MoneyController.instance.AddMoney(_spentMoney); // refunds at a 1/3 rate

        TowerCountChanged?.Invoke(false);
        gameObject.SetActive(false);
    }

    public bool TryUpgradeTower(){
        if(!GetIsUpgradeAvailable()){ // unnecessary, here just in case anything happens
            Debug.Log("No upgrade available");
            return false;
        }

        if(MoneyController.instance.TryUseMoney(_upgradeCost)){
            _upgradeLevel++;

            if(_upgradeLevel == 3 || _upgradeLevel == 5)
                ChangeToNextModel();

            _towerScriptableObject = _towerScriptableObject.nextLevelTower;
            _spentMoney += _upgradeCost / 3;

            UpdateTowerStats();
            _towerRange.UpdateSphereCollider(_range);
            DisplayRange();
            _upgradeParticle.Play();
            TowerUpgraded?.Invoke();
            return true;
        }
            
        else
            return false;
    }

    private void ChangeToNextModel(){
        for(int i = 0; i < _towerModels.Count; i++){
            if(_towerModels[i] == _currentModel){
                _currentModel.SetActive(false);
                _towerModels[i+1].SetActive(true);
                _currentModel = _towerModels[i+1];

                _towerAnimationController.SetActiveAnimator(_currentModel.GetComponent<Animator>());
                return;
            }
        }
    }

    private void UpdateTowerStats(){
        _towerName = _towerScriptableObject.towerName;
        _towerCost = _towerScriptableObject.towerCost;
        _shootingCD = _towerScriptableObject.shootingCD;
        _shotDamage = _towerScriptableObject.shotDamage;    
        _range = _towerScriptableObject.range;

        if(_towerScriptableObject.nextLevelTower != null)
            _upgradeCost = _towerScriptableObject.nextLevelTower.towerCost;
    }

    protected void LookAtTarget(){
        _shootingOriginTransform.transform.forward = ((_target.transform.position + Vector3.up * _shootingOriginTransform.position.y) - _shootingOriginTransform.position).normalized;
        _modelsParent.transform.LookAt(_target.transform);
    }

    protected virtual EnemyController GetFirstTargetInRange(){
        List<EnemyController> enemiesInRange = new List<EnemyController>();

        foreach(EnemyController enemy in SpawnedEnemiesHolder.instance.GetSpawnedEnemyList())
            enemiesInRange.Add(enemy);

        for(int i = 0; i < enemiesInRange.Count; i++){
            if(_targetsInRange.Contains(enemiesInRange[i]) && enemiesInRange[i].GetIsAlive()){
                // returns the first enemy thats in range
                _target = enemiesInRange[i];
                LookAtTarget();
                return enemiesInRange[i];
            }

        }

        _target = null;
        return null;

    }

    protected virtual void Shoot(){ // shoot the target, for SingleTarget towers
        if(_target == null){
            return;
        }
        
        _target.TakeDamage(_shotDamage);

        if(!_target.GetIsAlive()){
            _targetsInRange.Remove(_target);
            _target = null;
        }
        
        _towerAnimationController.PlayShootingAnimation();
        GameObject shotParticle = ObjectPool.instance.SpawnFromPool("ShotParticle", _shootingOriginTransform.position, _shootingOriginTransform.rotation);
    }

    protected IEnumerator ShootCoroutine(){
        _readyToShoot = false;
        yield return new WaitForSeconds(_shootingCD);
        _towerAnimationController.StopShootingAnimation();
        GetFirstTargetInRange();
        if(_target != null)
            Shoot();
        _readyToShoot = true;
    }

    private void OnLevelStarted(){
        _connectedToTile.RemoveTower();
        _connectedToTile = null;
        _readyToShoot = false;

        gameObject.SetActive(false);
    }

    #region Getters and Setters
    public TowerSO.TowerType GetTowerType(){
        return _towerScriptableObject.towerType;
    }

    public void SetPlacementVisualsPlacibility(bool toSet){
        _towerPlacementVisual.SetIsInBuildableZone(toSet);
    }

    public bool GetIsUpgradeAvailable(){
        return _towerScriptableObject.nextLevelTower != null;
    }

    public int GetTowerCost(){
        return _towerScriptableObject.towerCost;
    }

    public int GetTowerLevel(){
        return _upgradeLevel;
    }

    public int GetTowerDamage(){
        return (int) _shotDamage;
    }

    public float GetTowerShootingCD(){
        return _shootingCD;
    }

    public float GetTowerRange(){
        return _range;
    }

    public string GetTowerName(){
        return _towerName;
    }

    public TowerSO GetTowerScriptableObject(){
        return _towerScriptableObject;
    }

    public int GetSpentMoney(){
        return _spentMoney;
    }
    #endregion

}
