using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Units;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class PositionBase : MonoBehaviour
    {
        protected Dictionary<CustomerUnit, Coroutine> _coroutineOfUnit = new Dictionary<CustomerUnit, Coroutine>();
        protected CustomerUnit _customerUnitInPosition = null;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Customer"))
            {
                CustomerUnit triggeredCustomerUnit = other.GetComponent<CustomerUnit>();
                if(IsTriggeringWithUnitAvailable(triggeredCustomerUnit))
                    StartCoroutineOfUnit(triggeredCustomerUnit);
                else
                    return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Customer"))
            {
                CustomerUnit leftUnit = other.GetComponent<CustomerUnit>();
                EndCoroutineOfUnit(leftUnit);

                if(_customerUnitInPosition == null)
                    return;

                if(leftUnit != _customerUnitInPosition)
                    return;
                
                OnCustomerLeftPosition();
            }
        }

        protected void StartCoroutineOfUnit(CustomerUnit customerUnit)
        {
            if(_coroutineOfUnit.ContainsKey(customerUnit))
                return;
            Coroutine newCoroutine;
            newCoroutine = StartCoroutine(SetUnitIfStoppedRoutine(customerUnit));
            _coroutineOfUnit.Add(customerUnit, newCoroutine);
        }

        protected void EndCoroutineOfUnit(CustomerUnit customerUnit)
        {
            if(!_coroutineOfUnit.ContainsKey(customerUnit))
                return;
            Coroutine coroutineToEnd = _coroutineOfUnit[customerUnit];
            StopCoroutine(coroutineToEnd);
            _coroutineOfUnit.Remove(customerUnit);
        }

        public void SetCustomerInPosition(CustomerUnit customerInPosition)
        {
            _customerUnitInPosition = customerInPosition;
        }

        public void ClearPosition()
        {
            if(_customerUnitInPosition == null)
                return;
            EndCoroutineOfUnit(_customerUnitInPosition);

            SetCustomerInPosition(null);
        }

        protected abstract IEnumerator SetUnitIfStoppedRoutine(CustomerUnit unit);
        protected abstract bool IsTriggeringWithUnitAvailable(CustomerUnit triggeredCustomerUnit);
        protected abstract void OnCustomerLeftPosition();
    }
}