using UnityEngine;
using System;
using Chameleon.Game.ArcadeIdle.Helpers;
using DG.Tweening;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopBase : MonoBehaviour
    {
        [SerializeField] private PoopScriptableObject _poopScriptableObject;
        [SerializeField] private Transform _topOfPoopTransform;
        private PoopType _poopType = PoopType.NormalPoop;
        private Vector3 _initialScale;
        private Rigidbody _poopRb;
        private SphereCollider _poopCollider;
        private bool _isPlacedOnShow = false;
        private bool _passedFinalPosition = false;
        private bool _isBaseofBelongingSet = false;
        private UpgradeSkillName _targetUpgradeSkillName;

        private void Awake()
        {
            Initialize();
        }
        
        private void Start()
        {
            string poopTypeString = _poopType.ToString();
            string poopSellPriceString = poopTypeString + "SellPrice";
            _targetUpgradeSkillName = (UpgradeSkillName)System.Enum.Parse(typeof(UpgradeSkillName), poopSellPriceString, true);
        }

        private void OnEnable()
        {
            if(_poopRb != null && _poopCollider != null)
                SetPhysicsActive(true);
            else
            {
                Initialize();
                SetPhysicsActive(true);
            }
            gameObject.name = "Poop #" + UnityEngine.Random.Range(1000, 9999);
            Helpers.ParentRemover.instance.RemoveParentFrom(transform);
            SetPassedFinalPosition(false);
            SetIsOnShow(false);
        }

        private void OnDisable()
        {
            if(_poopRb != null && _poopCollider != null)
                SetPhysicsActive(false);
            transform.DOKill();
            transform.localScale = _initialScale;
        }

        public void DisablePoop()
        {
            gameObject.SetActive(false);
        }

        private void Initialize()
        {
            _initialScale = transform.localScale;
            _isBaseofBelongingSet = _poopScriptableObject.isBaseOfSet;
            _poopType = _poopScriptableObject.poopType;
            
            _poopRb = GetComponent<Rigidbody>();
            _poopCollider = GetComponent<SphereCollider>();
        }
        
        public void MoveToPosition(Vector3 targetPosition, Action onComplete = null)
        {
            SetPhysicsActive(false);
            JumpAnimator.instance.MoveTargetToPosition(transform, targetPosition, .15f, onComplete);
        }

        public void SetPassedFinalPosition(bool passedFinalPosition)
        {
            _passedFinalPosition = passedFinalPosition;
        }
        
        public bool GetPassedFinalPosition()
        {
            return _passedFinalPosition;
        }

        public void SetPhysicsActive(bool isActive)
        {
            _poopRb.useGravity = isActive;
            _poopRb.isKinematic = !isActive;
            _poopCollider.enabled = isActive;
        }

        public void ApplyForce(Vector3 forceDir)
        {
            _poopRb.AddForce(forceDir, ForceMode.Impulse);
        }

        public PoopType GetPoopType()
        {
            return _poopType;
        }

        public void SetIsOnShow(bool isOnShow)
        {
            _isPlacedOnShow = isOnShow;
        }

        public bool GetIsOnShow()
        {
            return _isPlacedOnShow;
        }

        public int GetPoopSellPrice()
        {
            int poopSetMultiplier = (int)(PoopSetByType.GetPoopSetByType(_poopType)) + 1;
            int sellUpgradeLevelMultiplier = (int)UpgradeSkillDict.upgradableSkillsDict[_targetUpgradeSkillName].GetCurrentLevelCoef();
            return 1 * poopSetMultiplier * sellUpgradeLevelMultiplier;
        }

        public Vector3 GetTopOfPoop()
        {
            return _topOfPoopTransform.position;
        }

        public bool GetIsBaseOfSet()
        {
            return _isBaseofBelongingSet;
        }
    }
}