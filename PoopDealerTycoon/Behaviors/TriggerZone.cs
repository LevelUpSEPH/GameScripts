using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Zones
{
    public class TriggerZone : MonoBehaviour
    {
        protected Vector3 _initialScale;
        protected Vector3 _expandedScale;
        private bool _playerTriggered = false;

        private void Start()
        {
            _initialScale = transform.localScale;
            _expandedScale = _initialScale * 1.1f;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(_playerTriggered)
                    return;
                OnPlayerEnteredTrigger();
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                OnPlayerExitTrigger();
                _playerTriggered = false;
            }
        }

        protected virtual void OnPlayerEnteredTrigger()
        {
            ScaleAnimator.instance.AnimateScaleTo(transform, _expandedScale, .1f);
        }

        protected virtual void OnPlayerExitTrigger()
        {
            ScaleAnimator.instance.AnimateScaleTo(transform, _initialScale, .1f);
        }
    }
}