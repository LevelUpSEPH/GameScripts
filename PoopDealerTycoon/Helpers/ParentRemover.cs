using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class ParentRemover : Singleton<ParentRemover>
    {
        private bool _isQuitting = false;

        public void RemoveParentFrom(Transform transformTarget)
        {
            if(_isQuitting)
                return;
            transformTarget.parent = null;
        }

        protected override void OnApplicationQuit()
        {
            _isQuitting = true;
            base.Awake();
        }
    }
}