using System;
using UnityEngine;

namespace Chameleon.Game.Scripts.Controller
{
    public class TutorialFrogGridContent : FrogGridContent
    {
        public static event Action<Transform, bool> TutorialFrogInitialized;
        public static event Action<bool> TutorialFrogClicked;
        
        [SerializeField] private bool _isFirstFrog = false;
        private bool _firstFrogFlung = false;

        protected override void Start()
        {
            base.Start();

            if(_isFirstFrog)
            {
                CanInteract = true;
            }
            else
            {
                CanInteract = false;
            }

            TutorialFrogInitialized?.Invoke(transform, _isFirstFrog);
        }

        private void OnEnable()
        {
            if(_isFirstFrog)
            {
                CanInteract = true;
            }
            else
            {
                CanInteract = false;
                FrogGridContent.TongueAnimationComplete += OnTongueAnimationComplete;
            }
        }

        private void OnDisable()
        {
            FrogGridContent.TongueAnimationComplete -= OnTongueAnimationComplete;
        }

        public override bool TryFlingTongue(GridHexXZ<GridObject> gridHex)
        {
            if(!_firstFrogFlung && !_isFirstFrog)
            {
                return false;
            }
            
            TutorialFrogClicked?.Invoke(_isFirstFrog);

            return base.TryFlingTongue(gridHex);
        }

        private void OnTongueAnimationComplete()
        {
            FrogGridContent.TongueAnimationComplete -= OnTongueAnimationComplete;

            _firstFrogFlung = true;
            CanInteract = true;
        }
    }
}