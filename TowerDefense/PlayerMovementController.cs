using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementController : MonoBehaviour
{    
    public static event Action<bool> MovementStateChanged;

    [SerializeField] private float _baseSpeed = 5f;
    private float _speed;
    private float _rotationSpeed = 6f;
    private bool _canRotate = true;
    private Transform _playerModelTransform;
    
    private bool _isMoving = false;
    private bool _isMoveAvailable = true;

    private FloatingJoystick joystick;
    private CharacterController _characterController;
    private PlayerAnimationController _playerAnimationController;

    private void Awake(){
        Initiliaze();
    }

    private void Update(){
        if(!_isMoveAvailable)
            return;
        MoveWithRotation();
    }

    private void Initiliaze() {
        if (TryGetComponent<CharacterController>(out CharacterController characterController))
            _characterController = characterController;
        else
            Debug.LogError("No CharacterController Found!!");
        if (TryGetComponent<PlayerAnimationController>(out PlayerAnimationController playerAnimationController))
            _playerAnimationController = playerAnimationController;
        else
            Debug.LogError("No animator controller found!!");

        joystick = InputController.instance.GetFloatingJoystick();
    }

    private void MoveWithRotation(){
        float inputX = joystick.Horizontal;
        float inputZ = joystick.Vertical;
        Vector3 moveDir = new Vector3(inputX, 0, inputZ);

        bool tempIsMoving = _isMoving;    
        
        if(CanMove(moveDir)) {            
            _characterController.SimpleMove(moveDir * _speed);
            if(_canRotate)
                Rotate(moveDir);
            _playerAnimationController.PlayMoveAnimation();
            //_playerAnimationController.SetAnimationSpeed(moveDir.magnitude);
        }
        else
            _playerAnimationController.StopMoveAnimation();

        if (tempIsMoving != _isMoving)
            MovementStateChanged?.Invoke(_isMoving);
    }

    private void Rotate(Vector3 targetDir) {
        Quaternion rotation = Quaternion.Lerp(_playerModelTransform.rotation, Quaternion.LookRotation(targetDir), _rotationSpeed * targetDir.magnitude * Time.deltaTime);
        _playerModelTransform.rotation = rotation;
    }
    
    private bool CanMove(Vector3 moveDir) {
        _isMoving = _isMoveAvailable && moveDir.magnitude > 0.1f;

        return _isMoving;
    }

    public bool GetIsMoving(){
        return _isMoving;
    }

    public void SetCanMove(bool canMove){
        _isMoveAvailable = canMove;
    }

    public void SetMovementSpeedCoefficient(float movementSpeedCoef){
        _speed = _baseSpeed * movementSpeedCoef;
    }

    public void SetPlayerModelTransform(Transform modelTransform){
        _playerModelTransform = modelTransform;
    }

    public void SetCanRotate(bool toSet){
        _canRotate = toSet;
    }

}
