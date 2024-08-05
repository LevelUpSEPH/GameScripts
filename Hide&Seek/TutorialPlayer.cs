using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialPlayer : Singleton<TutorialPlayer>
{
    // Drag to Move
    // Catch camouflaged target
    // Avoid enemy -> Catch target
    // Hide from boss -> catch target
    // player catches a target by themself -> play catch next target
    private Queue<LevelBehaviour.LevelTutorial> _tutorialQueue = new Queue<LevelBehaviour.LevelTutorial>();
    private void Start()
    {
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable()
    {
        LevelBehaviour.Started -= OnLevelStarted;
    }

    private void OnLevelStarted()
    {
        ClearQueue();
        PlayDragToMoveTutorial();
        InitializeQueue();
    }

    private void InitializeQueue()
    {
        foreach(LevelBehaviour.LevelTutorial levelTutorial in LevelController.instance.CurrentLevel.levelTutorials)
        {
            _tutorialQueue.Enqueue(levelTutorial);
        }
        
    }

    private void ClearQueue()
    {
        _tutorialQueue.Clear();
    }

    private void PlayDragToMoveTutorial()
    {
        TutorialController.instance.Play(TutorialType.Drag, PlayNextInQueue, "Drag to Move");
    }

    private void PlayNextInQueue()
    {
        if(_tutorialQueue.Count <= 0)
        {
            return;
        }

        LevelBehaviour.LevelTutorial nextLevelTutorial = _tutorialQueue.Dequeue();
        TutorialType tutorialType = nextLevelTutorial.tutorialType;
        string tutorialText = nextLevelTutorial.tutorialText;
        Debug.Log("Playing Tutorial : " + tutorialType + " with text : " + tutorialText);
        TutorialController.instance.Play(tutorialType, PlayNextInQueue, tutorialText);        
    }
}
