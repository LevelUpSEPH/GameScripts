using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingTargetController : TargetControllerBase
{
    [SerializeField] private MovingTargetScriptableObject _movingTargetSO;
    [SerializeField] private GameObject _characterModel;
    [SerializeField] private GameObject _caughtCharacterModel;
    private MovingTargetAnimator _movingTargetAnimator;
    private NavMeshAgent _targetAgent;
    private int _spawnedPrints = 0;
    
    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        EnableHidingCharacterModel();
        _caughtCharacterModel.SetActive(false);
        InLevelController.GameStarted += OnGameStarted;
        StartCoroutine(HandleSteps());
    }

    private void OnDisable()
    {
        InLevelController.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        StartMovement();
    }

    public override void StartGetCaught()
    {
        if(_isCaught)
            return;
        _isCaught = true;

        TryPlayCaughtParticle();
            
        EnableCaughtCharacterModel();
        StopAllCoroutines();
        HideCatchingUI();
        InvokeTargetGotCaught();

        _targetAgent.isStopped = true;
        //_movingTargetAnimator.SetIsMovingAnimationPlaying(false);
    }

    private void Initialize()
    {
        _targetAgent = GetComponent<NavMeshAgent>();
        _movingTargetAnimator = GetComponent<MovingTargetAnimator>();
        _targetAgent.speed = _movingTargetSO.movementSpeed;
        _maxCaughtPercentage = _movingTargetSO.maxCaughtPercentage;
    }

    protected override void EnableCaughtCharacterModel()
    {
        _characterModel.SetActive(false);
        _caughtCharacterModel.SetActive(true);
    }

    protected override void EnableHidingCharacterModel()
    {
        _characterModel.SetActive(false);
    }

    protected override void EnableShownCharacterModel()
    {
        _characterModel.SetActive(true);
        _movingTargetAnimator.SetMoveAnimationPlaying(Vector3.Distance(transform.position, _targetAgent.destination) > .5f);
    }

    private void StartMovement()
    {
        StartCoroutine(PositionPicker());
    }

    private IEnumerator HandleSteps()
    {
        while(true)
        {
            Vector3 tempPos = transform.position;
            yield return new WaitForSeconds(.25f);
            if(tempPos != transform.position)
                SpawnStep();
        }
    }

    private IEnumerator PositionPicker()
    {
        while(true)
        {
            Vector3 spotToMove;
            if(RandomPointPicker.instance.TryGetRandomSpotToMove(out Vector3 randomSpot))
            {
                spotToMove = randomSpot;
                spotToMove = new Vector3(spotToMove.x, transform.position.y, spotToMove.z);
            }
            else
            {
                spotToMove = transform.position;
            }
            _movingTargetAnimator.SetMoveAnimationPlaying((Vector3.Distance(spotToMove, transform.position) > 1f));
            _targetAgent.SetDestination(spotToMove);
            if(Vector3.Distance(transform.position, spotToMove) < 1f)
                yield return new WaitForSeconds(.5f);
            else
                yield return new WaitForSeconds(3f);
        }
    }

    private void SpawnStep()
    {
        string footprint;

        if(_spawnedPrints % 2 == 0)
            footprint = "LeftFootprint";
        else
            footprint = "RightFootprint";

        if(ObjectPool.TrySpawnFromPool<FootprintBehaviour>(footprint, transform.position, transform.rotation, out FootprintBehaviour footprintBehaviour))
        {
            footprintBehaviour.StartLifetime(5f);
            _spawnedPrints++;
        }
        
    }
    
}
