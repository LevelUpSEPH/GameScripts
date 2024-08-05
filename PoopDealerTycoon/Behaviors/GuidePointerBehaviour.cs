using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class GuidePointerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _visualsParent;
        private Vector3 _targetPosition;
        
        private void Update()
        {
            if(!TutorialArrowsController.instance.GetHasTarget())
            {
                SetVisualsActive(false);
                return;
            }

            _targetPosition = TutorialArrowsController.instance.GetCurrentTargetPosition();
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

        private void SetVisualsActive(bool isActive)
        {
            _visualsParent.SetActive(isActive);
        }
        
    }
}