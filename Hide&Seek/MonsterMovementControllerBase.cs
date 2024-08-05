using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterMovementControllerBase : MonoBehaviour
{
    protected NavMeshAgent _monsterAgent;
    public abstract void StartMovement();
    public abstract void StopMovement();

    protected virtual void Awake()
    {
        if(TryGetComponent<NavMeshAgent>(out NavMeshAgent monsterAgent))
        {
            _monsterAgent = monsterAgent;
        }
    }

    protected virtual void Start()
    {
        InLevelController.GameStarted += OnGameStarted;
    }

    protected virtual void OnDisable()
    {
        InLevelController.GameStarted -= OnGameStarted;
    }

    protected virtual void OnGameStarted()
    {
        StartMovement();
    }
    
}
