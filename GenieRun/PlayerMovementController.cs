using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour // On Player
{
    [SerializeField] private float _speed = 5;
    private float _moveSpeed = 0;
    private float _horizontalInput;
    private bool _canMove = false;
    [SerializeField] private float _planeBorder = 2.7f;
    private PlayerAnimationController _playerAnim;

    [SerializeField] private float _speedLimit = 15;
    private float _horizontalSpeed = 1.5f;

    private void Awake(){
        RegisterEvents();
    }

    private void RegisterEvents(){
        PlayerAnimationController.StartAnimationEnded += OnLevelStarted;
        /*InputController.Dragging += SetInput;
        InputController.DragEnd += ResetInput;*/
        FinishTrigger.ParkourFinished += OnParkourFinished;
        InputController.Dragging += OnDragging;
    }

    private void UnRegisterEvents(){
        PlayerAnimationController.StartAnimationEnded -= OnLevelStarted;
        /*InputController.Dragging -= SetInput;
        InputController.DragEnd -= ResetInput;*/
        FinishTrigger.ParkourFinished -= OnParkourFinished;
        InputController.Dragging -= OnDragging;
    }

    private void OnDisable(){
        UnRegisterEvents();
    }

    private void Start(){
        _playerAnim  = gameObject.GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        if (_canMove)
            MoveForward();
    }

    private void OnDragging(PointerEventData eventData) {
        if (_canMove)
            HandleSideMovement(eventData);
    }

    private void HandleSideMovement(PointerEventData eventData) {
        _horizontalInput = Mathf.Clamp(eventData.delta.x / 2, -_speedLimit, _speedLimit);

        transform.Translate(new Vector3(_horizontalInput, 0, 0) * Time.deltaTime * _horizontalSpeed);
    }

    private void MoveForward() {
        StayInLane();
        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }

    private void OnLevelStarted(){
        BeginMove();
    }
    
    private void OnParkourFinished(){
        EndMove();
    }

    private void BeginMove(){
        _canMove = true;
        _moveSpeed = _speed;
        _playerAnim.PlayWalkAnimation();
    }

    private void EndMove(){
        _moveSpeed = 0;
        _canMove = false;
        _playerAnim.StopWalkAnimation();
        gameObject.transform.DOMoveX(0, 0.2f);
    }

    private void Move(){
        Vector3 direction;
        direction = new Vector3(_horizontalInput, 0, _moveSpeed);
        transform.Translate(direction * Time.deltaTime/*, Space.World*/);
    }

    private void SetInput(PointerEventData eventData){
        _horizontalInput = SwipeController.instance.GetHorizontalInput();
    }

    private void ResetInput(Vector3 unused){
        _horizontalInput = 0;
    }

    private void StayInLane(){
        if(transform.position.x < -_planeBorder)
            transform.position = new Vector3(-_planeBorder, transform.position.y , transform.position.z);
        else if(transform.position.x > _planeBorder)
            transform.position = new Vector3(_planeBorder, transform.position.y, transform.position.z);
    }

}