using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Tutorial.Tutorials;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class TutorialArrowsController : Singleton<TutorialArrowsController>
    {
        [SerializeField] private TutorialArrowBehavior _arrowInScene;
        private Vector3 _currentTargetPosition;
        private bool _hasTarget;

        protected override void Awake()
        {
            base.Awake();
            PointToWorldWithArrowTutorial.TutorialStartedPlaying += OnTutorialStarted;
            Chameleon.Tutorial.StepSystem.Steps.Abstract.TutorialStep.TutorialStepCompleted += OnTutorialCompleted;
        }

        private void OnDisable()
        {
            PointToWorldWithArrowTutorial.TutorialStartedPlaying -= OnTutorialStarted;
            Chameleon.Tutorial.StepSystem.Steps.Abstract.TutorialStep.TutorialStepCompleted -= OnTutorialCompleted;
        }

        private void OnTutorialStarted(Vector3 tutorialPosition)
        {
            _currentTargetPosition = tutorialPosition;
            _arrowInScene.transform.position = tutorialPosition + Vector3.up * 2.5f;
            _hasTarget = true;
            _arrowInScene.gameObject.SetActive(true);
        }

        private void OnTutorialCompleted()
        {
            _hasTarget = false;
            _arrowInScene.gameObject.SetActive(false);
        }

        public bool GetHasTarget()
        {
            return _hasTarget;
        }
        
        public Vector3 GetCurrentTargetPosition()
        {
            return _currentTargetPosition;
        }

    }
}