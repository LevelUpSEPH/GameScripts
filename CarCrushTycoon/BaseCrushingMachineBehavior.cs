using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class BaseCrushingMachine : MonoBehaviour
    {
        [SerializeField] private CarDropZoneBehavior _targetCarDropZone;
        [SerializeField] private ScrapStashBehavior _scrapStash;
        [SerializeField] private Transform _topOfBeltTransform;
        [SerializeField] private Transform _finalTransformReference;
        [SerializeField] private Transform _scrapFinalTransform;
        [SerializeField] private Vector3 _beltDirection;
        [SerializeField] private ParticleSystem _carExplodeParticle;

        private CarController _carToTakeInAnimation;
        protected Transform _carToAnimate;

        private bool _isMachineAvailable = true;

        private float _timeToMoveToFinalTransform = 2.5f;

        private float _topOfBeltY;

        private void Start()
        {
            _targetCarDropZone.AvailabilityChanged += OnAvailabilityChanged;

            _topOfBeltY = _topOfBeltTransform.position.y;

            OnStart();
        }

        protected virtual void OnStart()
        {

        }

        private void OnDisable()
        {
            _targetCarDropZone.AvailabilityChanged -= OnAvailabilityChanged;
        }

        private void OnAvailabilityChanged(bool isAvailable)
        {
            if(isAvailable)
                return;

            DOVirtual.DelayedCall(1.5f, StartDestroyCarSequence);
        }

        private void StartDestroyCarSequence()
        {
            if(!_isMachineAvailable)
                return;

            _isMachineAvailable = false;

            ReplaceCarWithModel();
        }
        
        private void ReplaceCarWithModel()
        {
            if(_targetCarDropZone.TryGetParkedCar(out CarController carToTakeInAnimation))
            {
                _carToTakeInAnimation = carToTakeInAnimation;
            }
            else
            {
                _isMachineAvailable = true;
                return;
            }

            string carModelTag = _carToTakeInAnimation.GetCarModelTag();
            Vector3 carPosition = _carToTakeInAnimation.transform.position;
            Quaternion carRotation = _carToTakeInAnimation.transform.rotation;
            _carToTakeInAnimation.DisableCar();

            _carToAnimate = ObjectPool.instance.SpawnFromPool(carModelTag, carPosition, carRotation).transform;

            ResetCarToAnimate(_carToAnimate);
            
            MoveToDestructionArea();
        }

        private void MoveToDestructionArea()
        {
            Quaternion targetRotation = _finalTransformReference.rotation;
            Vector3 targetPosition = _finalTransformReference.position;

            _carToAnimate.DORotate(targetRotation.eulerAngles, _timeToMoveToFinalTransform / 2, RotateMode.Fast).SetEase(Ease.Linear);

            _carToAnimate.DOMove(targetPosition, _timeToMoveToFinalTransform).SetEase(Ease.Linear);

            DOVirtual.DelayedCall(_timeToMoveToFinalTransform + 1, DestroyCar);
        }

        private void DestroyCar()
        {
            PlayDestroyAnimation(ExplodeCarWithScraps);
        }

        private void ExplodeCarWithScraps()
        {
            _carToAnimate.gameObject.SetActive(false);
            PlayExplodeParticleAtPosition(_carToAnimate.position);

            // spawn some amount of scraps according to carController's car type
            ScrapWorth scrapWorth = CalculateScrapWorth(_carToTakeInAnimation);
            
            SpawnScraps(scrapWorth);
        }

        protected virtual ScrapWorth CalculateScrapWorth(CarController targetCar) // this will take carController as args to calculate
        {
            ScrapWorth scrapWorth = targetCar.GetScrapWorth();

            return scrapWorth;
        }

        private void SpawnScraps(ScrapWorth scrapWorth)
        {
            List<Transform> spawnedScraps = new List<Transform>();

            for(int i = 0; i < scrapWorth.cheapScrapWorth; i++)
            {
                spawnedScraps.Add(ObjectPool.instance.SpawnFromPool("CheapScrap", _carToAnimate.position, Quaternion.identity).transform);
            }
            for(int i = 0; i < scrapWorth.midScrapWorth; i++)
            {
                spawnedScraps.Add(ObjectPool.instance.SpawnFromPool("MidScrap", _carToAnimate.position, Quaternion.identity).transform);
            }
            for(int i = 0; i < scrapWorth.expensiveScrapWorth; i++)
            {
                spawnedScraps.Add(ObjectPool.instance.SpawnFromPool("ExpensiveScrap", _carToAnimate.position, Quaternion.identity).transform);
            }

            SpreadScraps(spawnedScraps);
        }

        private void SpreadScraps(List<Transform> scrapTransforms)
        {
            foreach(Transform scrapTransform in scrapTransforms)
            {
                float randomX = UnityEngine.Random.Range(-2.5f, 2.5f);
                float yPos = _topOfBeltY;
                float randomZ = UnityEngine.Random.Range(-2.5f, 2.5f);

                Vector3 randomDirection = new Vector3(randomX, scrapTransform.position.y, randomZ).normalized;
                Vector3 randomPos = new Vector3(scrapTransform.position.x + (randomDirection.x), yPos, scrapTransform.position.z + randomDirection.z);

                JumpAnimator.instance.MoveTargetToPosition(scrapTransform, randomPos, .5f, null);
            }

            DOVirtual.DelayedCall(1f, () => MoveScraps(scrapTransforms));
        }

        private void MoveScraps(List<Transform> scrapTransforms)
        {
            DOVirtual.DelayedCall(2f, () => AddScrapsToStash(scrapTransforms));

            foreach(Transform scrapTransform in scrapTransforms)
            {
                Vector3 targetPos = scrapTransform.position;

                if(Mathf.Abs(_beltDirection.x) > 0)
                {
                    targetPos.x = _scrapFinalTransform.position.x + UnityEngine.Random.Range(-1, 1);
                }
                else if(Mathf.Abs(_beltDirection.z) > 0)
                {
                    targetPos.z = _scrapFinalTransform.position.z + UnityEngine.Random.Range(-1, 1);
                }

                scrapTransform.DOMove(targetPos, 1.5f);
            }
        }
        
        private void AddScrapsToStash(List<Transform> scrapTransforms)
        {
            foreach(Transform scrapTransform in scrapTransforms)
            {
                Scrap scrap = scrapTransform.GetComponent<Scrap>();
                _scrapStash.PlaceScrapInZone(scrap);
            }

            EndDestroySequence();
        }

        private void EndDestroySequence()
        {
            _carToTakeInAnimation = null;
            _carToAnimate = null;
            
            _isMachineAvailable = true;

            _targetCarDropZone.ClearZone();
        }

        protected void PlayExplodeParticleAtPosition(Vector3 positionToPlayAt)
        {
            _carExplodeParticle.transform.position = positionToPlayAt;

            _carExplodeParticle.Play();
        }

        private void ResetCarToAnimate(Transform carToAnimate)
        {
            carToAnimate.localScale = Vector3.one;

            MeshRenderer[] meshRenderers = carToAnimate.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.color = Color.white;
            }
        }

        protected abstract void PlayDestroyAnimation(Action onCompletedDestroy);
    }
}