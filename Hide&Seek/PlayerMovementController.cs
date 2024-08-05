using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private float _rotationSpeed = 6f;
    [SerializeField] private Transform _playerModelTransform;
    
    private bool _isMoving = false;
    private bool _isMoveAvailable = true;

    private FloatingJoystick joystick;
    private PlayerAnimationController _playerAnimationController;

    private Rigidbody _rb;

    private void Awake()
    {
        Initiliaze();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void Update()
    {
        MoveWithRotation();
    }

    private void Initiliaze() 
    {
        if (TryGetComponent<PlayerAnimationController>(out PlayerAnimationController playerAnimationController))
            _playerAnimationController = playerAnimationController;
        else
            Debug.LogError("No animator controller found!!");

        joystick = InputController.instance.GetFloatingJoystick();
    }

    private void RegisterEvents()
    {
        InLevelController.LevelCompleted += OnLevelCompleted;
    }

    private void UnregisterEvents()
    {
        InLevelController.LevelCompleted -= OnLevelCompleted;
    }

    private void MoveWithRotation()
    {
        float inputX = joystick.Horizontal;
        float inputZ = joystick.Vertical;
        
        Vector3 moveDir = new Vector3(inputX, 0, inputZ);
        bool tempIsMoving = _isMoving;
        
        if(CanMove(moveDir)) {
            _isMoving = true;
            //_characterController.SimpleMove(moveDir * _speed);
            _rb.velocity = moveDir * _speed;
            Rotate(moveDir);
        }
        else{
            _rb.velocity = Vector3.zero;
            _isMoving = false;
        }

        if(_isMoving != tempIsMoving) // movementStateChanged
            _playerAnimationController.SetMoveAnimationPlaying(_isMoving);
    }

    private void Rotate(Vector3 targetDir) 
    {
        Quaternion rotation = Quaternion.Lerp(_playerModelTransform.rotation, Quaternion.LookRotation(targetDir), _rotationSpeed * targetDir.magnitude * Time.deltaTime);
        _playerModelTransform.rotation = rotation;
    }
    
    private bool CanMove(Vector3 moveDir) 
    {
        return _isMoveAvailable && moveDir.magnitude > 0 && InputController.instance.GetIsInputAvailable();
    }

    private void OnLevelCompleted(bool isSuccess)
    {
        _isMoveAvailable = isSuccess;
    }

    public bool GetIsMoving()
    {
        return _isMoving;
    }

    public void SetCanMove(bool canMove)
    {
        _isMoveAvailable = canMove;
    }
}
