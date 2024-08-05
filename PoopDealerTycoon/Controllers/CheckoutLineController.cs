using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Movement;
using RocketUtils.SerializableDictionary;
using Sirenix.OdinInspector;

namespace Chameleon.Game.ArcadeIdle.Checkout
{
    public class CheckoutLineController : Singleton<CheckoutLineController>
    {
        [SerializeField] private CheckoutLinePosition _checkoutLinePositionPrefab;
        [SerializeField] private CheckoutLinePosition _firstCheckoutPosition;
        [SerializeField] private CheckoutLineAvailability _checkoutLineAvailabilityDict = new CheckoutLineAvailability(); // line position, is full
        private List<CustomerUnit> _customerUnits = new List<CustomerUnit>();
        private List<CheckoutLinePosition> _checkoutLinePositionByIndex = new List<CheckoutLinePosition>();
        private CheckoutLinePosition _lastAddedPosition;

        private int _lastAddedPositionOrder = 2;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _checkoutLineAvailabilityDict.Add(_firstCheckoutPosition, false);
            _checkoutLinePositionByIndex.Add(_firstCheckoutPosition);
            _lastAddedPosition = _firstCheckoutPosition;

            _firstCheckoutPosition.SetIsFirstSpot(true);
            _firstCheckoutPosition.LinePositionAvailabilityChanged += OnLinePositionAvailabilityChanged;
        }

        private void OnDisable()
        {
            _firstCheckoutPosition.LinePositionAvailabilityChanged -= OnLinePositionAvailabilityChanged;
        }

        private void Update()
        {
            HandleLinePositions(); // error
        }

        private void HandleLinePositions()
        {
            for(int i = 1; i < _checkoutLineAvailabilityDict.Count; i++)
            {
                if(!GetIsLinePositionFull(i))
                    continue;
                if(GetIsLinePositionFull(i - 1))
                    continue;
                CustomerUnit unitInPosition = _checkoutLinePositionByIndex[i].GetCustomerUnitInPosition();
                SetLinePositionFull(i, false);
                _checkoutLinePositionByIndex[i].ClearPosition();
                if(unitInPosition != null)
                {
                    SetLinePositionFull(i - 1, true);
                    _checkoutLinePositionByIndex[i - 1].SetCustomerInPosition(unitInPosition);
                    MoveUnitToItsPosition(unitInPosition, _checkoutLinePositionByIndex[i - 1]); // error
                }
            }
        }

        [Button]
        public void ForceMoveAllUnitsForward()
        {
            for(int i = 0; i < _customerUnits.Count; i++)
            {
                BaseAIMovementController movementController = _customerUnits[i].GetComponent<BaseAIMovementController>();
                if(i == 0)
                {
                    if(_customerUnits[i].GetCameToFirstPlace())
                        continue;
                    SetLinePositionFull(i + 1, false);
                    MoveUnitToItsPosition(_customerUnits[i], _checkoutLinePositionByIndex[i]);
                    SetLinePositionFull(i, true);
                }
                else
                {
                    SetLinePositionFull(i, false);
                    movementController.MoveToPosition(_checkoutLinePositionByIndex[i - 1].transform.position);
                    SetLinePositionFull(i - 1, true);
                }
            }
        }

        private void MoveUnitToItsPosition(CustomerUnit customerUnit, CheckoutLinePosition targetPosition)
        {
            BaseAIMovementController movementController = customerUnit.GetComponent<BaseAIMovementController>(); // is null
            movementController.MoveToPosition(targetPosition.transform.position);
        }

        private void OnLinePositionAvailabilityChanged() // Update the line after line positions availability is changed
        {
            //HandleLinePositions();
        }

        public void AddUnitToCashoutList(CustomerUnit customerUnit)
        {
            if(customerUnit.GetIsInLine())
                return;
            _customerUnits.Add(customerUnit);
            customerUnit.CashedOut += OnUnitCashedOut;
            ExpandLineIfFull();

            for(int i = 0; i < _checkoutLinePositionByIndex.Count; i++)
            {
                if(!GetIsLinePositionFull(i))
                {
                    SetLinePositionFull(i, true);
                    _checkoutLinePositionByIndex[i].SetCustomerInPosition(customerUnit);
                    customerUnit.SetIsInLine(true);
                    MoveUnitToItsPosition(customerUnit, _checkoutLinePositionByIndex[i]);
                    break;
                }
            }
        }

        private void SetLinePositionFull(int lineIndex, bool isFull)
        {
            _checkoutLineAvailabilityDict[_checkoutLinePositionByIndex[lineIndex]] = isFull;
        }

        private bool GetIsLinePositionFull(int lineIndex)
        {
            return _checkoutLineAvailabilityDict[_checkoutLinePositionByIndex[lineIndex]];
        }

        private void OnUnitCashedOut()
        {
            CustomerUnit customerUnit = _customerUnits[0];
            customerUnit.CashedOut -= OnUnitCashedOut;
            _customerUnits.Remove(customerUnit);
            _checkoutLinePositionByIndex[0].ClearPosition();
            SetLinePositionFull(0, false);
        }

        private void ExpandLineIfFull()
        {
            foreach(var value in _checkoutLineAvailabilityDict.Values)
            {
                if(!value)
                    return;
            }
            
            CheckoutLinePosition nextPosition = InstantiateLinePosition();
            _checkoutLinePositionByIndex.Add(nextPosition);
            _checkoutLineAvailabilityDict.Add(nextPosition, false);
            _lastAddedPosition = nextPosition;
        }

        private CheckoutLinePosition InstantiateLinePosition()
        {
            CheckoutLinePosition checkoutLinePosition = Instantiate(_checkoutLinePositionPrefab, _firstCheckoutPosition.transform.parent);
            Vector3 nextPositionLocation = GetNextPositionForLine();
            checkoutLinePosition.transform.position = nextPositionLocation;

            checkoutLinePosition.gameObject.name = "CheckoutLinePosition #" + _lastAddedPositionOrder;
            _lastAddedPositionOrder++;

            return checkoutLinePosition;
        }

        private Vector3 GetNextPositionForLine()
        {
            Vector3 dirAwayFromCheckout = -_firstCheckoutPosition.transform.right;
            Vector3 nextPositionLocation = _lastAddedPosition.transform.position + dirAwayFromCheckout * 2f;

            return nextPositionLocation;
        }

        [System.Serializable]
        public class CheckoutLineAvailability : SerializableDictionary<CheckoutLinePosition, bool>{};
        
    }
}
