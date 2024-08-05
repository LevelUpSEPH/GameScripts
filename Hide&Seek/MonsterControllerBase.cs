using UnityEngine;

public class MonsterControllerBase : MonoBehaviour, ISeeker
{
    protected bool _canSeePlayer = false;
    protected MonsterMovementControllerBase _monsterMovementController;
    protected MonsterAnimationController _monsterAnimationController;
    protected bool _hasValidAnimationController => _monsterAnimationController != null;
    protected PlayerController _playerController;
    
    protected virtual void Start()
    {
        InLevelController.LevelCompleted += OnLevelCompleted;
        _monsterMovementController = GetComponent<MonsterMovementControllerBase>();
        if(TryGetComponent<MonsterAnimationController>(out MonsterAnimationController monsterAnimationController))
            _monsterAnimationController = GetComponent<MonsterAnimationController>();
    }

    protected virtual void OnDisable()
    {
        InLevelController.LevelCompleted -= OnLevelCompleted;
    }

    protected virtual void OnLevelCompleted(bool unused)
    {
        InLevelController.LevelCompleted -= OnLevelCompleted;
        _monsterMovementController.StopMovement();
        _monsterMovementController.enabled = false;
        this.enabled = false;
    }

    public void Catch(IHider hider){
        hider.StartGetCaught();
        if(_hasValidAnimationController)
                _monsterAnimationController.PlayCatchingAnimation();
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            if(!other.GetComponent<PlayerController>().GetIsPlayerHiding())
                Catch(other.GetComponent<IHider>());
        }
    }

    public bool GetCanSeePlayer(){
        return _canSeePlayer;
    }

    public void SetCanSeePlayer(bool canSeePlayer){
        _canSeePlayer = canSeePlayer;
        if(canSeePlayer)
            OnSawPlayer();
    }

    public void SetPlayerReference(PlayerController playerController)
    {
        _playerController = playerController;
    }

    protected virtual void OnSawPlayer()
    {
        
    }

    // Animation... Movement, Catching etc.
}
