using UnityEngine;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Units;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Helpers;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle
{
    public class NormalCustomerSpawner : BaseCustomerSpawner
    {
        [SerializeField] private int _maxCustomerForEachAvailableType = 5;
        [SerializeField] private int _maxCustomerInScene = 12;
        [SerializeField] private Transform _customerSpawnPositionsParent;
        private List<Transform> _customerSpawnPositions = new List<Transform>();
        private int _customerInScene = 0;

        private void OnEnable()
        {
            CustomerUnit.CustomerDisabled += OnCustomerDisabled;

            foreach(Transform child in _customerSpawnPositionsParent)
            {
                _customerSpawnPositions.Add(child);
            }
        }

        private void OnDisable()
        {
            CustomerUnit.CustomerDisabled -= OnCustomerDisabled;
        }

        protected override void SpawnCustomer()
        {
            PoopType[] poopTypes;
            if(PlayerData.Instance.TutorialSaveData.IsSequenceCompleted(Chameleon.Tutorial.StepSystem.Data.TutorialSequenceType.PlacePoopSequence))
            {
                poopTypes = GetRandomAvailableTypes();
            }
            else
            {
                poopTypes = new PoopType[1]{PoopType.NormalPoop};
            }
            if(poopTypes == null)
            {
                return;
            }
            Vector3 randomSpawnPos = GetRandomPosition();
            GameObject customer = ObjectPool.instance.SpawnFromPool("Customer", randomSpawnPos, Quaternion.identity);
            if(customer == null)
            {
                return;
            }
            CustomerUnit customerUnit = customer.GetComponent<CustomerUnit>();

            InitializeCustomer(customerUnit, poopTypes);
            _customerInScene++;
        }

        private Vector3 GetRandomPosition()
        {
            return _customerSpawnPositions[Random.Range(0, _customerSpawnPositions.Count - 1)].position;
        }

        private void OnCustomerDisabled()
        {
            _customerInScene--;
        }

        protected override int GetAmountToTake()
        {
            int randomNumber = Random.Range(0, 10);
            if(randomNumber < 5)
            {
                return 1;
            }
            else if(randomNumber >= 5 && randomNumber < 8)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        protected override bool CanSpawn()
        {
            return _customerInScene < Mathf.Min(_maxCustomerInScene, _maxCustomerForEachAvailableType * PoopTypeActivityController.instance.GetActivePoopTypeCount()) && base.CanSpawn();
        }

        private void InitializeCustomer(CustomerUnit customerUnit, PoopType[] poopTypes)
        {
            customerUnit.SetWants(poopTypes);
        }

        protected override bool GetIsPoopTypeUsable(PoopType poopType)
        {
            return base.GetIsPoopTypeUsable(poopType) &&
            Helpers.ScenePointManager.instance.GetPoopShowPlaceHasFreePosition(poopType);
        }
    }
}