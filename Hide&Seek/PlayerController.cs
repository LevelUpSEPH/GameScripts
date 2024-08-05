using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IHider, ISeeker, IColored, IColorable
{
    public static event Action PlayerGotCaught;
    public static event Action<bool> PlayerHidingStateChanged;
    [SerializeField] private Colors.Color _initialColor;
    [SerializeField] private SkinnedMeshRenderer _playerModelRenderer;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private GameObject _cageModel;
    
    private Vector3 _modelLocalPos;
    private Colors.Color _playerColor;
    private bool _isPlayerHiding = false; // player needs to be both in the same color as a wall and not moving to be able to hide, which means if hiding && !isMoving
    private PlayerMovementController _playerMovementController;
    private PlayerAnimationController _playerAnimationController;
    private GameObject _stickedWall = null;
    private bool _isStuckToWall = false;
    private bool _isCaught = false;

    private void Awake(){
        Initialize();
        Paint(_initialColor);
    }

    private void LateUpdate(){
        if(GetCanStickToWall()){
            StickToWall();
        }

        if(GetIsMoving() && _isStuckToWall){
            _isStuckToWall = false;
            ResetModelTransform();
            EndHide();
        }

    }

    private void Initialize(){
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerAnimationController = GetComponent<PlayerAnimationController>();
        _modelLocalPos = _playerModel.transform.localPosition;
    }

    private void StickToWall(){
        Vector3 nearestWallPoint = _stickedWall.GetComponent<Collider>().ClosestPointOnBounds(_playerModel.transform.position);
        Vector3 oppositeDirectionFromWall = (_playerModel.transform.position - nearestWallPoint).normalized;
        Quaternion rotation = Quaternion.LookRotation(oppositeDirectionFromWall);
        _playerMovementController.SetCanMove(false);
        _playerModel.transform.rotation = rotation;
        _playerModel.transform.position = nearestWallPoint + oppositeDirectionFromWall * .2f;
        _isStuckToWall = true;
        _playerMovementController.SetCanMove(true);
        PlayerHidingStateChanged?.Invoke(true);
    }

    private bool GetCanStickToWall(){
        return _isPlayerHiding && !_playerMovementController.GetIsMoving() && _stickedWall != null && !_isStuckToWall;
    }

    public void Catch(IHider hider){
        hider.StartGetCaught();
    }

    public void StartGetCaught(){
        StartCoroutine(GetCaught());
    }

    private IEnumerator GetCaught() {
        if (_isCaught)
            yield return null;
        if (!_isPlayerHiding) {
            _isCaught = true;
            yield return new WaitForSeconds(1f);            
            PlayerGotCaught?.Invoke();
            _cageModel.SetActive(true);
            _playerAnimationController.PlayCageSmokeParticle();
        }        
    }

    public void Paint(Colors.Color color){
        _playerColor = color;
        _playerModelRenderer.material = MaterialHolder.instance.GetMaterialOfColor(color);
        _playerAnimationController.PlaySmokeParticle(color);
    }

    private bool TryHideWithColor(Colors.Color otherColor){
        if(otherColor == _playerColor){
            SetIsPlayerHiding(true);
            return true;
        }

        else{
            SetIsPlayerHiding(false);
            return false;
        }
    }

    private void EndHide(){
        if(!_isPlayerHiding)
            return;
        SetIsPlayerHiding(false);
        PlayerHidingStateChanged?.Invoke(false);
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Collectable")){
            other.GetComponent<ICollectable>().Collect();
        }
        
        if(other.CompareTag("Wall")){
            TryHideWithColor(other.GetComponent<IColored>().GetColor());
            _stickedWall = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Wall")){
            _stickedWall = null;
            SetIsPlayerHiding(false);
        }
    }

    public Colors.Color GetColor(){
        return _playerColor;
    }

    public void SetPlayerColor(Colors.Color colorToSet){
        _playerColor = colorToSet;
        _playerModelRenderer.material = MaterialHolder.instance.GetMaterialOfColor(colorToSet);
    }

    public bool GetIsPlayerHiding(){
        return _isPlayerHiding && !_playerMovementController.GetIsMoving();
    }

    public void SetIsPlayerHiding(bool toSet){
        _isPlayerHiding = toSet;
        _playerAnimationController.SetIsHidden(_isPlayerHiding);
    }

    public bool GetIsMoving(){
        return _playerMovementController.GetIsMoving();
    }

    public void SetCanMove(bool canMove)
    {
        _playerMovementController.SetCanMove(canMove);
    }

    private void ResetModelTransform(){
        _playerModel.transform.localPosition = _modelLocalPos;
    }

}
