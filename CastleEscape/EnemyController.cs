using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyController : Unit
{
    private Unit _enemyUnit;
    private FieldofView _fieldOfView;
    [SerializeField] private GameObject _unitStatusParticle;
    [Tooltip("These weapons will be hidden upon fear")]
    [SerializeField] private List<GameObject> _weapons;
    [SerializeField] private Animator enemyAnimator;
    private bool _canMove = true;
    private bool _isFeared = false;

    public static event Action<Unit> EnemyGotScared;

    private void Awake(){
        _fieldOfView = GetComponent<FieldofView>();
        _enemyUnit = GetComponent<Unit>();
        _unitAnimator = enemyAnimator;
    }

    private void Update(){
        if(_isFeared)
            return;
        if(_fieldOfView.GetSawPlayer()){
            if(_fieldOfView.GetPlayerRef() != null)
                SawPlayer();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(TagCheck(other)){
            HandleFight(other);
        }
    }

    protected override bool TagCheck(Collider other)
    {
        return other.gameObject.CompareTag("Player");
    }

    private void SawPlayer(){
        _canMove = false;
        GameObject player = _fieldOfView.GetPlayerRef();
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(_unitLevel > playerController.GetLevel() && !_isAttacking && !playerController.GetIsPlayerHiding()){
            _isAttacking = true;
            playerController.Die();
            PlayAttackAnimation();
            StartCoroutine(HandleAttackCooldown());
        }
        else if(_unitLevel < playerController.GetLevel()){
            EnemyGotScared?.Invoke(this);
            _isFeared = true;
            PlayFearAnimation();
            HideWeapons();
            Vector3 particlePlayPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z + 1.5f);
            Instantiate(_unitStatusParticle, particlePlayPosition, Quaternion.identity, transform);
        }
            
            
    }

    protected override void HandleFight(Collider other)
    {
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = playerRef.GetComponent<PlayerController>();
            
            if(_unitLevel >= playerController.GetLevel() && !_isAttacking && !playerController.GetIsPlayerHiding()){
                _isAttacking = true;
                PlayAttackAnimation();
                StartCoroutine(HandleAttackCooldown());
            }
            
            else if(_unitLevel < playerController.GetLevel())
            {
                Die();
                _canMove = false;
            }
    }

    private void HideWeapons(){
        foreach(GameObject weapon in _weapons)
            weapon.SetActive(false);
    }

    public bool GetCanMove(){
        return _canMove;
    }

}
