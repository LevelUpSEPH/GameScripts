using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Unit
{
    public static event Action<Transform> PlayerModelChanged;
    public static event Action AnyKeyUsed;
    private List<Key.KeyType> _keyList;

    [SerializeField] private List<GameObject> _playerModel;
    [SerializeField] private ParticleSystem _levelUpParticle;
    private GameObject _currentPlayerModel;
    private bool _isPlayerHiding;
    private void Awake(){
        _keyList = new List<Key.KeyType>();
    }

    private void Start(){
        UpgradeItem.AnyUpgradeReceived += OnAnyUpgradeReceived;
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        _currentPlayerModel = playerMovement.GetPlayerModelTransform().gameObject;
        _unitAnimator = _currentPlayerModel.GetComponent<Animator>();
    }

    private void OnDisable(){
        UpgradeItem.AnyUpgradeReceived -= OnAnyUpgradeReceived;
    }

    private void OnAnyUpgradeReceived(){
        PlayLevelUpParticle();
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        Transform playerModelTransform = playerMovement.GetPlayerModelTransform();
        _currentPlayerModel = Instantiate(_playerModel[_unitLevel/2 - 1], playerModelTransform.position, playerModelTransform.rotation, transform );
        Destroy(playerModelTransform.gameObject);
        _unitAnimator = _currentPlayerModel.GetComponent<Animator>();
        PlayerModelChanged?.Invoke(_currentPlayerModel.transform);
    }

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Key")){
            Key key = other.gameObject.GetComponent<Key>();
            Key.KeyType keyType = key.GetKeyType();
            AddKeyToList(keyType);
            key.CollectedKey();
        }

        if(other.gameObject.CompareTag("Door")){
            DoorWithKey door = other.gameObject.GetComponent<DoorWithKey>();
            foreach(Key.KeyType keyType in _keyList)
                if(door.TryToUnlockWith(keyType)){
                    _keyList.Remove(keyType);
                    AnyKeyUsed?.Invoke();
                    break;
                }
        }

        if(TagCheck(other))
            HandleFight(other);
        
    }

    protected override void HandleFight(Collider other)
    {
        Unit targetUnit = other.gameObject.GetComponent<Unit>();
            
            if(_unitLevel >= targetUnit.GetLevel() && !_isAttacking){
                _isAttacking = true;
                PlayAttackAnimation();
                StartCoroutine(HandleAttackCooldown());
            }
            
            else if(_unitLevel < targetUnit.GetLevel() && !_isPlayerHiding)
            {
                Die();
            }
    }

    private void AddKeyToList(Key.KeyType keyType){
        _keyList.Add(keyType);
    }

    public int GetOrangeKeyCount(){
        int orangeKeyCount = 0;
        foreach(Key.KeyType keyType in _keyList)
            if(keyType == Key.KeyType.OrangeKey)
                orangeKeyCount++;
        return orangeKeyCount;
    }

    public int GetBlueKeyCount(){
        int blueKeyCount = 0;
        foreach(Key.KeyType keyType in _keyList)
            if(keyType == Key.KeyType.BlueKey)
                blueKeyCount++;
        return blueKeyCount;
    }

    public int GetPurpleKeyCount(){
        int purpleKeyCount = 0;
        foreach(Key.KeyType keyType in _keyList)
            if(keyType == Key.KeyType.PurpleKey)
                purpleKeyCount++;
        return purpleKeyCount;
    }

    public int GetRedKeyCount(){
        int redKeyCount = 0;
        foreach(Key.KeyType keyType in _keyList)
            if(keyType == Key.KeyType.RedKey)
                redKeyCount++;
        return redKeyCount;
    }

    protected override bool TagCheck(Collider other){
        return other.gameObject.CompareTag("Enemy");
    }

    public bool GetIsPlayerHiding(){
        return _isPlayerHiding;
    }

    public void HidePlayer(){
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.SetCanMove(false);

        CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = false;
        
        _isPlayerHiding = true;
        _currentPlayerModel.SetActive(false);
    }

    public void ShowPlayer(){
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.SetCanMove(true);

        CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = true;

        _isPlayerHiding = false;
        _currentPlayerModel.SetActive(true);
    }

    public override void Die()
    {
        base.Die();
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.SetCanMove(false);
    }

    private void PlayLevelUpParticle(){
        _levelUpParticle.Play();
    }

}
