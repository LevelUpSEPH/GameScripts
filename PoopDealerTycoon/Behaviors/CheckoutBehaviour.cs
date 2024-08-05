using System.Collections;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Helpers;
using System;

namespace Chameleon.Game.ArcadeIdle.Checkout
{
    public class CheckoutBehaviour : MonoBehaviour
    {
        public static event Action CheckoutActivated;
        public static event Action UnitCheckedOut;
        [SerializeField] private MoneyStackingArea _moneyStackingArea;
        [SerializeField] private Transform _packagingSpotTransform;
        private Quaternion _boxRotation;
        private Vector3 _boxPosition;
        private bool _isCheckoutActive = false;
        private bool _isCheckingOut = false;
        private bool _isPermanentCheckoutActive = false;

        private void Awake()
        {
            Initialize();
        }
        
        private void Start()
        {
            CheckoutActivated?.Invoke();
            CustomerUnit.CameToFirstPlace += OnFirstPlaceArrived;
        }

        private void OnDisable()
        {
            CustomerUnit.CameToFirstPlace -= OnFirstPlaceArrived;
        }

        private void Initialize()
        {
            _boxPosition = _packagingSpotTransform.position;
            _boxRotation = _packagingSpotTransform.rotation;
        }

        private void OnFirstPlaceArrived(CustomerUnit customerUnit)
        {
            StartCoroutine(StartCheckoutOfFirstInLine(customerUnit));
        }

        private bool CanCheckout()
        {
            return !_isCheckingOut && (_isCheckoutActive || _isPermanentCheckoutActive);
        }

        private IEnumerator StartCheckoutOfFirstInLine(CustomerUnit customerUnit)
        {
            while(true)
            {
                yield return null;
                if(!CanCheckout())
                {
                    continue;
                }
                _isCheckingOut = true;
                
                AnimateCheckoutSequence(customerUnit);
                break;
            }
        }

        private void AnimateCheckoutSequence(CustomerUnit customerUnit)
        {
            Vector3 boxFinalPosition = customerUnit.GetEveryPoop()[0].transform.position;
            GameObject box = ObjectPool.instance.SpawnFromPool("Box", _boxPosition, _boxRotation);
            
            PoopBase[] poops = customerUnit.GetEveryPoop().ToArray();
            int i = poops.Length;

            CheckAndContinueAnimation();

            void CheckAndContinueAnimation()
            {
                i--;
                if(i - 1 >= 0)
                {
                    JumpAnimator.instance.MoveTargetToPosition(poops[i].transform, _packagingSpotTransform.position, .4f, 4, CheckAndContinueAnimation);
                }
                else if(i - 1 < 0)
                {
                    JumpAnimator.instance.MoveTargetToPosition(poops[i].transform, _packagingSpotTransform.position, .4f, 4, CoverBox);
                }
            }

            void CoverBox()
            {
                BoxAnimator boxAnimator = box.GetComponent<BoxAnimator>();
                boxAnimator.AnimateCover(() => {
                    foreach(PoopBase poop in poops)
                        poop.DisablePoop();
                    ReturnBox();
                });
            }

            void ReturnBox()
            {
                JumpAnimator.instance.MoveTargetToPosition(box.transform, boxFinalPosition, .4f, OnCheckoutComplete);
            }                

            void OnCheckoutComplete()
            {
                customerUnit.SetCarryingBox(box);
                UnitCheckedOut?.Invoke();
                TurnPoopsIntoMoney(poops);
                customerUnit.InvokeCashedOut();
                _isCheckingOut = false;
            }
        }

        private void TurnPoopsIntoMoney(PoopBase[] poops)
        {
            int earnedMoney = EarnedMoneyController.CalculateTotalEarnedMoney(poops);
            _moneyStackingArea.AddMoney(earnedMoney);
        }

        public void SetCheckoutActive(bool isCheckoutActive)
        {
            if(_isPermanentCheckoutActive)
                return;
            _isCheckoutActive = isCheckoutActive;
        }

        public void SetPermanentCheckoutActive(bool isPermanentCheckoutActive)
        {
            _isPermanentCheckoutActive = isPermanentCheckoutActive;
        }

        public bool GetIsPermanentCheckoutActive()
        {
            return _isPermanentCheckoutActive;
        }
    }
}
