using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _playerModel;
    [SerializeField] private float _baseShootingRange = 3f;
    [SerializeField] private ParticleSystem _upgradeParticle;
    private PlayerShootingController _playerShootingController;
    private PlayerMovementController _playerMovementController;

    private void Awake(){
        InitializeControllers();

        InitializeAttributes();
    }

    private void Start(){
        RegisterEvents();
    }

    private void OnDisable(){
        UnRegisterEvents();
    }

    private void RegisterEvents(){
        UpgradeController.AttackDamageUpgraded += SetAttackDamage;
        UpgradeController.AttackRangeUpgraded += SetAttackRange;
        UpgradeController.AttackSpeedUpgraded += SetAttackSpeed;
        UpgradeController.MovementSpeedUpgraded += SetMovementSpeed;

        UpgradeController.AttackDamageUpgraded += OnAnythingUpgraded;
        UpgradeController.AttackRangeUpgraded += OnAnythingUpgraded;
        UpgradeController.AttackSpeedUpgraded += OnAnythingUpgraded;
        UpgradeController.MovementSpeedUpgraded += OnAnythingUpgraded;
    }

    private void UnRegisterEvents(){
        UpgradeController.AttackDamageUpgraded -= SetAttackDamage;
        UpgradeController.AttackRangeUpgraded -= SetAttackRange;
        UpgradeController.AttackSpeedUpgraded -= SetAttackSpeed;
        UpgradeController.MovementSpeedUpgraded -= SetMovementSpeed;

        UpgradeController.AttackDamageUpgraded -= OnAnythingUpgraded;
        UpgradeController.AttackRangeUpgraded -= OnAnythingUpgraded;
        UpgradeController.AttackSpeedUpgraded -= OnAnythingUpgraded;
        UpgradeController.MovementSpeedUpgraded -= OnAnythingUpgraded;
    }

    private void Update(){
        _playerMovementController.SetCanRotate(!_playerShootingController.GetHasTargetInRange()); // doesnt have to be in update, will fix
    }

    private void InitializeControllers(){
        _playerShootingController = GetComponent<PlayerShootingController>();
        _playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void InitializeAttributes(){
        _playerShootingController.SetBaseAttackRange(_baseShootingRange);
        SetAttackDamage();
        SetAttackSpeed();
        SetAttackRange();
        SetMovementSpeed();
        _playerMovementController.SetPlayerModelTransform(_playerModel);
        _playerShootingController.SetPlayerModelTransform(_playerModel);
    }

    private void PlayUpgradeParticle(){
        _upgradeParticle.Play();
    }

    public void SetAttackDamage(){
        _playerShootingController.SetAttackDamageCoefficient(UpgradeController.instance.attackDamage[PlayerData.Instance.AttackDamageLevel - 1].coef);
    }

    public void SetAttackSpeed(){
        _playerShootingController.SetAttackSpeedCoefficient(UpgradeController.instance.attackSpeed[PlayerData.Instance.AttackSpeedLevel - 1].coef);
        
    }

    public void SetAttackRange(){
        _playerShootingController.SetAttackRangeCoefficient(UpgradeController.instance.attackRange[PlayerData.Instance.AttackRangeLevel - 1].coef);
        
    }

    public void SetMovementSpeed(){
        _playerMovementController.SetMovementSpeedCoefficient(UpgradeController.instance.movementSpeed[PlayerData.Instance.MovementSpeedLevel - 1].coef);
        
    }

    public void OnAnythingUpgraded(){
        PlayUpgradeParticle();
    }

}
