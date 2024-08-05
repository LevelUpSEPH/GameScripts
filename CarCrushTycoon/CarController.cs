using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private CarStats _carStats;
        [SerializeField] private CarInputTranslator _targetCarInputTranslator;
        [SerializeField] private Transform _seatTransform;
        [SerializeField] private Transform _carLeavePosition;
        [SerializeField] private Transform _enterPercentageTargetPos;
        [SerializeField] private string _carModelTag;
        private Transform _sittingUnit = null;
        private bool _isCarBeingEntered = false;

        private bool _isCarActive = false;

        private void Start()
        {
            _targetCarInputTranslator.SetWheelStats(_carStats.maxSteeringAngle, _carStats.maxSpeed, _carStats.acceleration);
        }

        private void OnDisable()
        {
            ResetCarController();
        }

        public void SeatUnit(Transform unitTransform)
        {
            _sittingUnit = unitTransform;
            unitTransform.SetParent(_seatTransform);
            unitTransform.localPosition = Vector3.zero;
            unitTransform.localRotation = Quaternion.identity;
        }

        public void ReleaseUnit()
        {
            _sittingUnit.SetParent(null);
            _sittingUnit.GetComponent<BaseUnitController>().LeaveCar();
            _sittingUnit.transform.position = _carLeavePosition.position;
            _sittingUnit = null;
        }

        private void ResetCarController()
        {
            SetEnteredUnit(null);
            SetCarBeingEntered(false);
            SetIsCarActive(false);
        }

        public void SetTransform(Vector3 targetPosition, Quaternion targetRotation)
        {
            _targetCarInputTranslator.SetTransform(targetPosition, targetRotation);
        }

        public bool GetCanEnterCar()
        {
            return GetIsCarActive();
        }
        
        public void SetIsCarActive(bool isActive)
        {
            _isCarActive = isActive;
        }

        public bool GetIsCarActive()
        {
            return _isCarActive;
        }

        public void SetCarBeingEntered(bool isCarBeingEntered)
        {
            _isCarBeingEntered = isCarBeingEntered;
        }

        public bool GetIsCarBeingEntered()
        {
            return _isCarBeingEntered;
        }

        public Transform GetEnterPercentageTargetTransform()
        {
            return _enterPercentageTargetPos;
        }

        public void SetEnteredUnit(BaseUnitController enteredUnit)
        {
            _targetCarInputTranslator.SetTargetUnit(enteredUnit);
        }

        public bool GetIsOnForward()
        {
            return _targetCarInputTranslator.GetIsOnForward();
        }

        public void ToggleIsOnForward()
        {
            _targetCarInputTranslator.ToggleIsOnForward();
        }

        public void SetIsOnForward(bool isOnForward)
        {
            _targetCarInputTranslator.SetIsOnForward(isOnForward);
        }

        public string GetCarModelTag()
        {
            return _carModelTag;
        }

        public void DisableCar()
        {
            _targetCarInputTranslator.DisableCar();
        }

        public ScrapWorth GetScrapWorth()
        {
            return _carStats.scrapWorth;
        }

    }
}