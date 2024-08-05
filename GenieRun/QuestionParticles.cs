using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestionParticles : MonoBehaviour
{
    private void Start(){
        InLevelController.LevelStarted += OnLevelStarted;
    }

    private void OnDestroy() {
        InLevelController.LevelStarted -= OnLevelStarted;
    }

    private void OnLevelStarted(){
        gameObject.SetActive(false);
    }

}
