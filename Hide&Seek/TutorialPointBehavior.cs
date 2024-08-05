using System;
using UnityEngine;

public class TutorialPointBehavior : MonoBehaviour
{
    public static event Action<Transform> TutorialPointInitialized;
    public static event Action TutorialPointRemoved;

    private void Start()
    {
        PlayerController.PlayerHidingStateChanged += OnPlayerHidingStateChanged;
        TutorialPointInitialized?.Invoke(transform);
    }

    private void OnDisable()
    {
        PlayerController.PlayerHidingStateChanged -= OnPlayerHidingStateChanged;
        TutorialPointRemoved?.Invoke();
    }

    private void OnPlayerHidingStateChanged(bool isHiding)
    {
        if(isHiding)
            RemoveTutorialPoint();
    }

    private void RemoveTutorialPoint()
    {
        TutorialPointRemoved?.Invoke();
        gameObject.SetActive(false);
    }
    
}
