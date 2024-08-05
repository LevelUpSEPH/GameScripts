using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle
{
    public class SpecialCustomerSpawner : BaseCustomerSpawner
    {
        [SerializeField] private SpecialCustomerBehaviour _specialCustomer;
        [SerializeField] private Transform _specialCustomerSpawnPositionTransform;
        [SerializeField] private Transform _specialCustomerTargetTransform;

        private bool _hasSpecialCustomerActive = false;

        protected override void Start()
        {
            base.Start();
            SpecialCustomerBehaviour.SpecialCustomerLeft += OnSpecialCustomerLeft;
        }

        private void OnDisable()
        {
            SpecialCustomerBehaviour.SpecialCustomerLeft -= OnSpecialCustomerLeft;
        }

        private void OnSpecialCustomerLeft()
        {
            _hasSpecialCustomerActive = false;
        }

        protected override bool CanSpawn()
        {
            return Input.GetKeyDown(KeyCode.J) && !_hasSpecialCustomerActive && base.CanSpawn() && PlayerData.Instance.PlayerSawZoneOpen > 0;
        }

        protected override void SpawnCustomer()
        {
            PoopType[] poopTypes = GetRandomAvailableTypes();
            if(poopTypes == null)
            {
                return;
            }
            if(_specialCustomer == null)
            {
                return;
            }
            if(poopTypes.Length <= 1)
            {
                return;
            }
            _hasSpecialCustomerActive = true;
            _specialCustomer.transform.position = _specialCustomerSpawnPositionTransform.position;
            _specialCustomer.gameObject.SetActive(true);
            _specialCustomer.SetWants(poopTypes);
            _specialCustomer.MoveTowards(_specialCustomerTargetTransform.position);
        }

        protected override int GetAmountToTake()
        {
            return Random.Range(5, 11);
        }
    }
}