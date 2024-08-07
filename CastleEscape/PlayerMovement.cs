using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField] private Transform _playerModelTransform;

    [SerializeField] private float _speed = 5f;
    private Unit _playerUnit;

    private bool _isMoving = false;

    private bool _canMove = true;


    private void Awake(){
        _characterController = GetComponent<CharacterController>();
        _playerUnit = GetComponent<Unit>();
    }

    private void Start(){
        PlayerController.PlayerModelChanged += OnPlayerModelChanged;
        FinishTrigger.FinishTriggered += OnFinishTriggered;
    }

    private void OnDisable(){
        PlayerController.PlayerModelChanged -= OnPlayerModelChanged;
        FinishTrigger.FinishTriggered -= OnFinishTriggered;
    }

    private void Update(){
        MoveWithRotation();
    }

    private void OnPlayerModelChanged(Transform newPlayerModelTransform){
        PlayerController playerController = GetComponent<PlayerController>();
        _playerModelTransform = newPlayerModelTransform;
    }

    private void MoveWithRotation(){
        float inputX = FloatingJoystick.Instance.Horizontal;
        float inputZ = FloatingJoystick.Instance.Vertical;
        if(Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0){
            _isMoving = true;
            _playerUnit.PlayMoveAnimation();
        }
            
        else{
            _isMoving = false;
            _playerUnit.StopMoveAnimation();
        }
            
        Vector3 moveDir = new Vector3(inputX, 0, inputZ);

        float rotationSpeed = 6f;
        if(moveDir.magnitude > 0){
            Quaternion rotation = Quaternion.Lerp(_playerModelTransform.rotation, Quaternion.LookRotation(moveDir), rotationSpeed * moveDir.magnitude * Time.deltaTime);
            _playerModelTransform.rotation = rotation;
        }
        if (_canMove) {
            //_characterController.SimpleMove(moveDir * _speed);
            _characterController.SimpleMove(moveDir * _speed);
        }
            
    }

    public Transform GetPlayerModelTransform(){
        return _playerModelTransform;
    }

    public bool GetIsMoving(){
        return _isMoving;
    }

    public void SetCanMove(bool canMove){
        _canMove = canMove;
    }

    private void OnFinishTriggered(){
        _canMove = false;
    }
}
