using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovementController : MonsterMovementControllerBase // can be reworked or made from scratch
{

    private enum MovementState{
        Patrolling,
        SpecialChasing,
        Chasing,
        Waiting
    }

    [SerializeField] private float _chaseTime = 5f;
    [SerializeField] private float _chaseCooldown = 10f;
    private MovementState _monsterMovementState = MovementState.Patrolling;
    private float _timeUntilChase;
    private bool _isAfterPlayer;
    private MonsterController _monsterController;
    
    private Coroutine _positionPickRoutine;
    private Coroutine _lookForPlayerRoutine;
    private Coroutine _chasePlayerTimeRoutine;
    
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        _monsterController.OnMonsterMovementStateChanged(false);
    }

    private void Initialize()
    {
        _monsterController = GetComponent<MonsterController>();
    }

    private void Update()
    {
        HandleAgentActivation();
        HandlePlayerDetection();
        HandleSpecialChasing();
        HandleLookForPlayerFromAfar();
        
        HandleAnimation();
        HandleRotation();

    }

    private void HandleLookForPlayerFromAfar()
    {
        if(GetShouldStop())
        {
            SetDestination(transform.position, false);
            _monsterMovementState = MovementState.Patrolling;
        }
    }

    private bool GetShouldStop()
    {
        return _isAfterPlayer && 
        Vector3.Distance(transform.position, _monsterAgent.destination) < 2 && 
        !_monsterController.GetCanSeePlayer() && 
        _monsterMovementState != MovementState.SpecialChasing;
    }

    private void SetDestination(Vector3 targetPos, bool isAfterPlayer)
    {
        Vector3 relativeTargetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        _monsterAgent.destination = relativeTargetPos;
        _isAfterPlayer = isAfterPlayer;
    }

    private void HandleAnimation()
    {
        float remainingDistance = Vector3.Distance(_monsterAgent.destination, transform.position);
        if(remainingDistance > 1f && !_monsterAgent.isStopped)
        {
            _monsterController.OnMonsterMovementStateChanged(true);
        }
        else if(_monsterAgent.isStopped || remainingDistance < 1f)
        {
            _monsterController.OnMonsterMovementStateChanged(false);
        }
    }

    private void HandlePlayerDetection()
    {
        if(_monsterController.GetCanSeePlayer()){
            _monsterMovementState = MovementState.Chasing;
            SetDestination(InLevelController.instance.GetPlayerPosition(), true);
            ResetChaseCooldown();
        }
        else if(_monsterMovementState == MovementState.Chasing)
        {
            _monsterMovementState = MovementState.Patrolling;
        }
    }

    private void HandleAgentActivation()
    {
        _monsterAgent.isStopped = (_monsterMovementState == MovementState.Waiting);
    }

    private void HandleSpecialChasing()
    {
        if(_monsterMovementState != MovementState.SpecialChasing)
            return;
        
        float remainingDistance = Vector3.Distance(_monsterAgent.transform.position, _monsterAgent.destination);
        if(remainingDistance <= 2f){
            StopCoroutine(_lookForPlayerRoutine);
            SetDestination(transform.position, false);
            _monsterMovementState = MovementState.Patrolling;
            _monsterController.OnMonsterMovementStateChanged(false);
            _lookForPlayerRoutine = StartCoroutine(LookForPlayerRoutine());
            _positionPickRoutine = StartCoroutine(PositionPickRoutine());
            StopCoroutine(_chasePlayerTimeRoutine);
        }
        else
        {
            SetDestination(InLevelController.instance.GetPlayerPosition(), true);
        }
    }

    private void HandleRotation()
    {
        if(_monsterMovementState != MovementState.Waiting && _monsterAgent.isStopped == true)
            return;
        float distToPlayer = Vector3.Distance(transform.position, InLevelController.instance.GetPlayerPosition());
        if(distToPlayer < 2f){
            Quaternion playerRotation = Quaternion.LookRotation(InLevelController.instance.GetPlayerPosition() - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * 5f);
        }
    }

    private IEnumerator PositionPickRoutine()
    {
        while(true){
            yield return new WaitForSeconds(2f);
            if(_monsterMovementState != MovementState.Patrolling)
                continue;
            Debug.Log("Setting movement state to waiting");
            _monsterMovementState = MovementState.Waiting;
            _monsterController.OnMonsterMovementStateChanged(false);
            yield return new WaitForSeconds(2f);
            if(_monsterMovementState != MovementState.Waiting)
                continue;
            Debug.Log("Setting movement state to patrolling");
            _monsterMovementState = MovementState.Patrolling;
            _monsterController.OnMonsterMovementStateChanged(true);
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
            SetDestination(spotToMove, false);
        }
    }

    private IEnumerator LookForPlayerRoutine()
    {
        ResetChaseCooldown();
        while(true){
            yield return new WaitForSeconds(1);
            _timeUntilChase--;
            if(_monsterController.GetCanSeePlayer())
                ResetChaseCooldown();
            if(_timeUntilChase <= 0)
            {
                SpecialChasePlayer();
                break;
            }
        }
    }

    private void SpecialChasePlayer()
    {
        StopCoroutine(_positionPickRoutine);
        StopCoroutine(_lookForPlayerRoutine);
        Debug.Log("Starting periodic chase");
        SetDestination(InLevelController.instance.GetPlayerPosition(), true);
        _monsterMovementState = MovementState.SpecialChasing;

        _chasePlayerTimeRoutine = StartCoroutine(ChasePlayerTime());
    }

    private IEnumerator ChasePlayerTime()
    {
        yield return new WaitForSeconds(_chaseTime);
        _monsterMovementState = MovementState.Patrolling;

        _positionPickRoutine = StartCoroutine(PositionPickRoutine());
        _lookForPlayerRoutine = StartCoroutine(LookForPlayerRoutine());
    }

    public bool GetIsMonsterStopped()
    {
        return _monsterAgent.isStopped;
    }

    private void ResetChaseCooldown()
    {
        _timeUntilChase = _chaseCooldown;
    }

    public override void StartMovement()
    {
        _positionPickRoutine = StartCoroutine(PositionPickRoutine());
        _lookForPlayerRoutine = StartCoroutine(LookForPlayerRoutine());
    }

    public override void StopMovement()
    {
        _monsterAgent.isStopped = true;
    }

}
