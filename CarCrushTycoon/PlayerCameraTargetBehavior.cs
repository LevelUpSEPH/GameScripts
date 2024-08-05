using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PlayerCameraTargetBehavior : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        private void Update()
        {
            CopyPlayerPosition();
        }

        private void CopyPlayerPosition()
        {
            transform.position = _playerTransform.position;
        }
    }
}