using UnityEngine;
using UnityEngine.AI;
using Chameleon.Game.ArcadeIdle.Abstract;
using System;

namespace Chameleon.Game.ArcadeIdle.Movement
{
    public class BaseAIMovementController : BaseMovementController
    {
        public event Action ReachedDestination;

        protected NavMeshAgent _agent;
        private bool _hasDestination = false;

        protected override void Initialize()
        {
            base.Initialize();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            HandleReachedDestination();
        }

        private void HandleReachedDestination()
        {
            float distanceToDestination = Vector3.Distance(_agent.destination, transform.position);
            if(_hasDestination && distanceToDestination <= .01f)
            {
                _hasDestination = false;
                _isMoving = false;
                SetMoveAnimationPlaying(false);
                ReachedDestination?.Invoke();
            }
        }
        
        public void MoveToPosition(Vector3 position)
        {
            _agent.destination = position;
            SetMoveAnimationPlaying(true);
            _hasDestination = true;
            _isMoving = true;
        }
    }
}