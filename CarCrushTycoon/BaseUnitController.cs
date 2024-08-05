using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class BaseUnitController : MonoBehaviour
    {
        protected BaseMovementController _movementController;
        protected CarController _enteredCar;

        private List<Scrap> _collectedScraps = new List<Scrap>();
        [SerializeField] private int _maxScrapCarryAmount = 3;
        [SerializeField] private List<Transform> _scrapCarryingPositions = new List<Transform>();
        public bool IsCarryingScrap => _collectedScraps.Count > 0;
        private Coroutine _turnInScrapRoutine = null;
        private Coroutine _collectScrapRoutine = null;

        private void Start()
        {
            _movementController = GetComponent<BaseMovementController>();

            _movementController.SetCanMove(true);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(GetIsInCar())
                return;

            if(other.CompareTag("Car") && !IsCarryingScrap)
            {
                CarController carController = other.GetComponent<CarController>();

                if(carController.GetCanEnterCar())
                {
                    OnEnteredCarTrigger(carController);
                }
            }
            else if(other.CompareTag("ScrapCollectionZone"))
            {
                if(_collectScrapRoutine != null)
                    return;

                ScrapCollectionZone scrapCollectionZone = other.GetComponent<ScrapCollectionZone>();
                _collectScrapRoutine = StartCoroutine(StartCollectScrapFromZone(scrapCollectionZone));
            }
            else if(other.CompareTag("Checkout"))
            {
                CheckoutBehavior checkout = other.GetComponent<CheckoutBehavior>();
                if(_turnInScrapRoutine == null && IsCarryingScrap)
                {
                    _turnInScrapRoutine = StartCoroutine(StartTurnInScrap(checkout));
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(GetIsInCar())
            {
                return;
            }

            if(other.CompareTag("Car"))
            {
                OnExitCarTrigger();
            }
            else if(other.CompareTag("ScrapCollectionZone"))
            {
                if(_collectScrapRoutine == null)
                    return;

                StopCoroutine(_collectScrapRoutine);
                _collectScrapRoutine = null;
            }
            else if(other.CompareTag("Checkout"))
            {
                if(_turnInScrapRoutine != null)
                {
                    StopCoroutine(_turnInScrapRoutine);
                    _turnInScrapRoutine = null;
                }
            }
        }

        private IEnumerator StartCollectScrapFromZone(ScrapCollectionZone scrapCollectionZone)
        {
            ScrapStashBehavior scrapStash = scrapCollectionZone.GetTargetScrapStash();

            while(_collectedScraps.Count < _maxScrapCarryAmount)
            {
                if(_movementController.GetIsMoving())
                {
                    yield return new WaitForSeconds(.1f);
                    continue;
                }
                    
                if(scrapStash.TryCollectScrapFromZone(out Scrap collectedScrap))
                {
                    CollectScrap(collectedScrap);

                    yield return new WaitForSeconds(.25f);
                }
                else
                {
                    yield return new WaitForSeconds(.1f);
                }
            }
        }

        private void CollectScrap(Scrap scrapToCollect)
        {
            scrapToCollect.transform.SetParent(transform);

            Chameleon.Game.ArcadeIdle.Helpers.JumpAnimator.instance.MoveTargetToPosition(scrapToCollect.transform, _scrapCarryingPositions[_collectedScraps.Count].position, duration: .5f, onComplete: OnJumpCompleted);

            void OnJumpCompleted()
            {
                scrapToCollect.transform.position = _scrapCarryingPositions[_collectedScraps.Count].position;
                _collectedScraps.Add(scrapToCollect);
            }
        }

        private IEnumerator StartTurnInScrap(CheckoutBehavior checkout)
        {
            while(IsCarryingScrap)
            {
                Scrap scrapToTurnIn = _collectedScraps[_collectedScraps.Count - 1];
                _collectedScraps.RemoveAt(_collectedScraps.Count - 1);
                
                scrapToTurnIn.transform.SetParent(null);
                checkout.TurnScrapIn(scrapToTurnIn);
                
                yield return new WaitForSeconds(.25f);
            }
        }

        private bool GetIsInCar()
        {
            return _enteredCar != null;
        }

        public BaseMovementController GetMovementController()
        {
            return _movementController;
        }

        protected virtual void OnEnteredCar(CarController enteredCar)
        {
            _enteredCar = enteredCar;

            _movementController.SetCanMove(false);

            _enteredCar.SeatUnit(transform);

            enteredCar.SetEnteredUnit(this);
        }

        public void LeaveCar()
        {
            OnLeftCar();
        }

        protected virtual void OnLeftCar()
        {
            _enteredCar.SetEnteredUnit(null);

            _enteredCar = null;

            _movementController.SetCanMove(true);
        }

        protected abstract void OnEnteredCarTrigger(CarController triggeredCar);

        protected abstract void OnExitCarTrigger();
    }
}