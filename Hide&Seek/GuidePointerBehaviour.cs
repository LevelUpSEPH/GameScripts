using UnityEngine;

public class GuidePointerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _visualsParent;
    private Vector3 _targetPosition;
    private bool _isGameActive = false;

    private void Start()
    {
        SetVisualsActive(false);
        InLevelController.GameStarted += OnGameStarted;
        InLevelController.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        InLevelController.GameStarted -= OnGameStarted;
        InLevelController.LevelCompleted -= OnLevelCompleted;
    }
    
    private void Update()
    {
        if(InLevelController.instance.TargetCountLeftToCatch <= 0 || !_isGameActive)
        {
            if(_visualsParent.activeInHierarchy)
                SetVisualsActive(false);
            return;
        }
        Transform tutorialPoint = InLevelController.instance.GetTutorialTargetPoint();
        if(tutorialPoint != null)
            _targetPosition = tutorialPoint.position;
        else
            _targetPosition = InLevelController.instance.GetTargetsLeftToCatch()[0].transform.position;
        HandleVisualsActivation();
        HandlePointerRotation();
    }

    private void HandlePointerRotation()
    {
        Vector3 dirToTarget = _targetPosition - transform.position;
        dirToTarget = new Vector3(dirToTarget.x, 0, dirToTarget.z);
        float rotationSpeed = 6f;
        Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToTarget), rotationSpeed * dirToTarget.magnitude * Time.deltaTime);
        transform.rotation = rotation;
    }

    private void HandleVisualsActivation()
    {
        if(Vector3.Distance(_targetPosition, transform.position) < 4f)
        {
            SetVisualsActive(false);
        }
        else
        {
            SetVisualsActive(true);
        }
    }

    private void OnGameStarted()
    {
        _isGameActive = true;
    }

    private void OnLevelCompleted(bool unused)
    {
        _isGameActive = false;
        SetVisualsActive(false);
    }

    private void SetVisualsActive(bool isActive)
    {
        _visualsParent.SetActive(isActive);
    }

    
}
