using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class LeavePositionsController : MonoBehaviour
    {
        [SerializeField] private Transform _leavePositionsParent;
        private List<Vector3> _leavePositions = new List<Vector3>();

        private void Start()
        {
            Initialize();
        }   

        private void Initialize()
        {
            foreach(Transform child in _leavePositionsParent)
            {
                _leavePositions.Add(child.position);
            }
        }

        public Vector3 GetRandomLeavePosition()
        {
            int leavePositionCount = _leavePositions.Count;
            int randomLeavePositionIndex = Random.Range(0, leavePositionCount);
            return _leavePositions[randomLeavePositionIndex];
        }
    }
}