using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour // Must run before EnemyController does
{
    [SerializeField] private float _rotationSpeed = 3f;
    private AIPathController _aiPathController;

    private List<Transform> _enemyPath = new List<Transform>();
    private float _movementSpeed = 5f;
    private bool _isStopped = false;
    private Vector3 _targetPosition;

    private void OnEnable(){
        InitializePath();
        
        SetNextPathNode();
        _isStopped = false;
    }

    private void OnDisable(){
        _isStopped = true;
        _enemyPath.Clear();
    }

    private void Update(){
        if(_isStopped)
            return;

        HandleRotation();

        if(Vector3.Distance(transform.position, _targetPosition) <= 0.2f)
            SetNextPathNode();

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);
    }

    private void InitializePath(){
        _aiPathController = GameObject.FindGameObjectWithTag("Path").GetComponent<AIPathController>();
        foreach(Transform node in _aiPathController.GetPath()){
            _enemyPath.Add(node);
        }
    }

    private void HandleRotation(){
        Vector3 direction = _targetPosition - transform.position;
        Quaternion rot = Quaternion.identity;

        if (direction != Vector3.zero) {
            rot = Quaternion.LookRotation(direction);
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotationSpeed);
    }

    private void SetNextPathNode(){
        if(_enemyPath.Count <= 0)
            return;

        SetDestination(_enemyPath[0].position);
        HandleRotation();

        _enemyPath.Remove(_enemyPath[0]);

        
    }

    private void SetDestination(Vector3 targetPosition){
        _targetPosition = targetPosition;
    }

    public void SetNavIsStopped(bool isStopped){
        _isStopped = isStopped;
    }

    public void SetMovementSpeed(float movementSpeed){
        _movementSpeed = movementSpeed;
    }

}
