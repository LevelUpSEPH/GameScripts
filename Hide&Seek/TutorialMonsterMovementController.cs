using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialMonsterMovementController : MonsterMovementControllerBase
{
    public static event Action MonsterMovedToFinalPos;
    public static event Action MonsterReachedPlayer;

    [SerializeField] private Transform _monsterTargetTransformAfterPlayer;
    private MonsterController _monsterController;

    private bool _isTargetingFinalPos = false;
    private bool _isLookingForPlayer = false;
    
    protected override void Start()
    {
        _monsterAgent.isStopped = true;
        _monsterController = GetComponent<MonsterController>();
        PlayerController.PlayerHidingStateChanged += OnPlayerHidingStateChanged;
        InLevelController.LevelCompleted += OnLevelCompleted;
    }

    protected override void OnDisable()
    {
        PlayerController.PlayerHidingStateChanged -= OnPlayerHidingStateChanged;
        InLevelController.LevelCompleted -= OnLevelCompleted;
        QuickTip.TipClosed -= MoveToLastPosition;
    }

    private void OnLevelCompleted(bool isSuccess)
    {
        StopMovement();
    }

    private void OnPlayerHidingStateChanged(bool isHiding)
    {
        if(isHiding)
            StartMovement();
    }

    private void Update()
    {
        if(_monsterAgent.isStopped)
            return;
        float remainingDistance = Vector3.Distance(_monsterAgent.destination, transform.position);
        if(remainingDistance < 2f && !_isTargetingFinalPos && !_isLookingForPlayer)
        {
            _monsterAgent.destination = transform.position;
            LookForPlayer();
        }
        if(_isTargetingFinalPos && remainingDistance < .5f)
        {
            StopMovement();
            MonsterMovedToFinalPos?.Invoke();
        }
    }

    public override void StartMovement()
    {
        _monsterAgent.destination = InLevelController.instance.GetPlayerPosition();
        _monsterController.OnMonsterMovementStateChanged(true);
        _monsterAgent.isStopped = false;
    }

    public override void StopMovement()
    {
        if(_monsterAgent.isActiveAndEnabled)
            _monsterAgent.isStopped = true;
        _monsterController.OnMonsterMovementStateChanged(false);
        PlayerController.PlayerHidingStateChanged -= OnPlayerHidingStateChanged;
        gameObject.SetActive(false);
    }

    private void LookForPlayer()
    {
        _isLookingForPlayer = true;
        _monsterController.OnMonsterMovementStateChanged(false);
        _monsterAgent.isStopped = true;
        MonsterReachedPlayer?.Invoke();
        QuickTip.TipClosed += MoveToLastPosition;
    }

    private void MoveToLastPosition()
    {
        QuickTip.TipClosed -= MoveToLastPosition;
        _monsterAgent.destination = _monsterTargetTransformAfterPlayer.position;
        _isTargetingFinalPos = true;
        _monsterAgent.isStopped = false;
        _monsterController.OnMonsterMovementStateChanged(true);
    }

}
