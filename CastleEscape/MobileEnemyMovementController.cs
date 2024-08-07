using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobileEnemyMovementController : MonoBehaviour
{
    private List<Vector3> _movementNodes;
    [SerializeField] private Transform _pathParent;

    [SerializeField] private float _patrolInterval = 0.5f;
    private Vector3 _movingToPos;
    private int _pathCounter = 0;

    private bool _isMoving = false;
    
    private NavMeshAgent _enemyAgent;
    private EnemyController _enemyController;

    private void Awake(){
        _movementNodes = new List<Vector3>();
        _enemyAgent = GetComponent<NavMeshAgent>();
        foreach(Transform child in _pathParent){
            _movementNodes.Add(child.transform.position);
        }
        _enemyController = GetComponent<EnemyController>();
    }

    private void Start(){
        StartCoroutine(MoveToPosition(_movementNodes[_pathCounter]));
    }

    private void Update(){
        if(!_enemyController.GetCanMove()){
            _enemyAgent.isStopped = true;
            return;
        }
            
        if(HasArrived()){
            _isMoving = false;
        }

        if(_enemyAgent.velocity.magnitude > 0)
            _enemyController.PlayMoveAnimation();
        else
            _enemyController.StopMoveAnimation();
            
        if(_isMoving){
            return;
        }

        StartCoroutine(MoveToPosition(_movementNodes[_pathCounter]));

    }

    private bool HasArrived(){
        float stoppingDistance = 0.1f;
        return Vector3.Distance(transform.position, _movingToPos) <= stoppingDistance;
    }

    private IEnumerator MoveToPosition(Vector3 position){
        _movingToPos = _movementNodes[_pathCounter];
        _isMoving = true;

        yield return new WaitForSeconds(_patrolInterval);
        _enemyAgent.SetDestination(position);

        if(_pathCounter < _movementNodes.Count - 1)
            _pathCounter++;
        else if(_pathCounter >= _movementNodes.Count - 1)
            _pathCounter = 0;
    }
}
