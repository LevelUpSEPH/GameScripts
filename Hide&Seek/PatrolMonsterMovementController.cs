using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMonsterMovementController : MonsterMovementControllerBase
{
    [SerializeField] private Transform _pathParent;
    [SerializeField] private float _patrolInterval = 0.5f;
    private List<Vector3> _movementNodes;
    private Vector3 _movingToPos;
    private int _pathCounter = 0;
    private PatrolMonsterController _monsterController;
    private bool _canMove = false;

    private bool _isMoving = false;

    protected override void Awake(){
        base.Awake();
        InitializeController();

        _movementNodes = new List<Vector3>();
        foreach(Transform child in _pathParent){
            _movementNodes.Add(child.transform.position);
        }
    }

    private void InitializeController()
    {
        if(TryGetComponent<PatrolMonsterController>(out PatrolMonsterController patrolMonsterController))
        {
            _monsterController = patrolMonsterController;
        }
        else
        {
            Debug.LogWarning("No monstercontroller attached");
        }
    }

    private void Update(){
        if(!_canMove)
            return;
        if(HasArrived()){
            _isMoving = false;
        }

        HandleMovementAnimation();
            
        if(_isMoving){
            return;
        }
        StartCoroutine(MoveToPosition(_movementNodes[_pathCounter]));
    }

    private void HandleMovementAnimation()
    {
        if(_monsterAgent.velocity.magnitude > 0)
            _monsterController.OnMonsterMovementStateChanged(true);
        else
            _monsterController.OnMonsterMovementStateChanged(false);
    }

    private bool HasArrived(){
        float stoppingDistance = 0.1f;
        return Vector3.Distance(transform.position, _movingToPos) <= stoppingDistance;
    }

    private IEnumerator MoveToPosition(Vector3 position){
        if(!_isMoving)
        {
            _movingToPos = _movementNodes[_pathCounter];
            _isMoving = true;

            yield return new WaitForSeconds(_patrolInterval);
            _monsterAgent.SetDestination(position);

            if(_pathCounter < _movementNodes.Count - 1)
                _pathCounter++;
            else if(_pathCounter >= _movementNodes.Count - 1)
                _pathCounter = 0;
        }
    }

    public override void StartMovement()
    {
        _canMove = true;
    }

    public override void StopMovement()
    {
    }
}
