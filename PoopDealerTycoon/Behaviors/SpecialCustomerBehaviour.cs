using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle
{
    public class SpecialCustomerBehaviour : MonoBehaviour
    {
        public static event Action<SpecialCustomerBehaviour> SpecialCustomerSpawned;
        public static event Action SpecialCustomerArrived;
        public static event Action SpecialCustomerLeft;
        public event Action<PoopType> ReceivedPoop;

        [SerializeField] private SpecialCustomerStockPlace _poopStockPlace;
        [SerializeField] private int _waitingTime = 90;
        [SerializeField] private MoneyStackingArea _moneyStackingArea;
        [SerializeField] private int _sellPriceMultiplier;
        private CustomerNeeds _customerNeeds = new CustomerNeeds();
        private Vector3 _initialPos;
        private bool _isCoroutineActive = false;

        private int _countdownTime;

        private void OnEnable()
        {
            Initialize();
            
            _poopStockPlace.PoopReceived += OnPoopReceived;
        }

        private void OnDisable()
        {
            _poopStockPlace.PoopReceived -= OnPoopReceived;

            _poopStockPlace.ResetSlots();
            _customerNeeds.ResetNeeds();
        }

        private void Initialize()
        {
            _countdownTime = _waitingTime;
            _initialPos = transform.position;
            _isCoroutineActive = false;

            SpecialCustomerSpawned?.Invoke(this);
        }

        public void MoveTowards(Vector3 targetPosition)
        {
            transform.DOMove(targetPosition, 3f).OnComplete(() => SpecialCustomerArrived?.Invoke());
        }

        private void OnPoopReceived(PoopType poopType)
        {
            _customerNeeds.RemoveFromNeeds(poopType);
            ReceivedPoop?.Invoke(poopType);
            if(_customerNeeds.GetNeeds().Count <= 0)
            {
                StopAllCoroutines();
                Leave();
            }
        }

        public void SetWants(PoopType[] poopTypes)
        {
            for(int i = 0; i < poopTypes.Length; i++)
            {
                PoopSlotWithType poopSlot = _poopStockPlace.GetPoopSlotAtIndex(i);
                if(poopTypes[i] == PoopType.None)
                {
                    poopSlot.SetSlotActive(false);
                    continue;
                }
                poopSlot.SetAcceptedPoopType(poopTypes[i]);
                _customerNeeds.AddToNeeds(poopTypes[i]);
                poopSlot.SetSlotActive(true);
                if(!_isCoroutineActive)
                    StartCoroutine(StartTimeBeforeLeave());
            }
            for(int i = poopTypes.Length; i < _poopStockPlace.GetSlotCount(); i ++)
            {
                PoopSlotWithType poopSlot = _poopStockPlace.GetPoopSlotAtIndex(i);
                poopSlot.SetSlotActive(false);
            }
        }

        private IEnumerator StartTimeBeforeLeave()
        {
            _isCoroutineActive = true;
            while(_countdownTime > 0)
            {
                yield return new WaitForSeconds(1);
                _countdownTime--;
            }
            Leave();
        }

        private void Leave()
        {
            SpawnMoney();
            transform.DOMove(_initialPos, 3f).OnComplete(() => DisableCustomer());
        }

        private void SpawnMoney()
        {
            PoopBase[] poops = _poopStockPlace.GetEveryPoop().ToArray();
            int earnedMoney = Helpers.EarnedMoneyController.CalculateTotalEarnedMoney(poops);
            _moneyStackingArea.AddMoney(earnedMoney * _sellPriceMultiplier);
        }

        private void DisableCustomer()
        {
            SpecialCustomerLeft?.Invoke();
            gameObject.SetActive(false);
        }

        public float GetRemainingTimePercentage()
        {
            return (float)_countdownTime/_waitingTime;
        }

        public int GetRemainingTime()
        {
            return _countdownTime;
        }

        public List<PoopType> GetNeeds()
        {
            return _customerNeeds.GetNeeds();
        }
    }
}