using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.Scripts.Controller
{
    public class RotatingFrogGridContent : FrogGridContent
    {
        [SerializeField] private float _rotationInterval;
        [SerializeField] private bool _rotatesClockwise = true;

        protected override void Start()
        {
            HandleRotates();
            base.Start();
        }

        private void HandleRotates()
        {
            StartCoroutine(RotateFrogRoutine());
        }

        private IEnumerator RotateFrogRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_rotationInterval);

                if(CanInteract)
                    RotateFrog();
            }
        }

        private void RotateFrog()
        {
            int tongueDirectionIndex = (int)_tongueDirection;
            float rotationAngle;
            if (_rotatesClockwise)
            {
                tongueDirectionIndex++;                
                rotationAngle = 60;
            }
            else
            {
                tongueDirectionIndex--;
                rotationAngle = -60;
            }
            tongueDirectionIndex %= 6;
            SetTongueDirection((TongueDirection)tongueDirectionIndex);
            transform.Rotate(Vector3.up, rotationAngle);
        }
    }
}