using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public abstract class BaseCustomerSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnCustomerCooldown = 5f;
        private PoopTypeNumerable _poopTypeNumerable = new PoopTypeNumerable();

        protected virtual void Start()
        {
            StartCoroutine(SpawnCustomerRoutine());
        }

        protected IEnumerator SpawnCustomerRoutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(_spawnCustomerCooldown);
                if(!CanSpawn())
                    continue;
                SpawnCustomer();
            }
        }

        protected abstract void SpawnCustomer();

        protected PoopType[] GetRandomAvailableTypes()
        {
            PoopType[] poopTypes = new PoopType[GetAmountToTake()];
            PopulatePoopTypesArray(poopTypes);
            
            if(IsPoopTypesValid(poopTypes))
                return poopTypes;
            return null;
        }

        protected abstract int GetAmountToTake();

        private void PopulatePoopTypesArray(PoopType[] poopTypes)
        {
            int customerSlotsFilled = 0;
            int maxForCustomer = poopTypes.Length;
            List<PoopType> availablePoopTypes = new List<PoopType>(GetUsablePoopTypes());
            int slotsToFill = maxForCustomer;
            int numberOfFailedTries = 0;

            while(customerSlotsFilled < slotsToFill)
            {
                int amountToGetOfType = Random.Range(0, maxForCustomer + 1);
                if(amountToGetOfType == 0)
                {
                    numberOfFailedTries++;
                    if(numberOfFailedTries >= 3)
                        break;
                    continue;
                }

                if(availablePoopTypes.Count <= 0)
                    break;
                PoopType poopTypeToGet = availablePoopTypes[Random.Range(0, availablePoopTypes.Count)];
                maxForCustomer -= amountToGetOfType;
                int maxToGetTemp = amountToGetOfType + customerSlotsFilled;
                for(int i = customerSlotsFilled; i < maxToGetTemp; i++)
                {
                    poopTypes[i] = poopTypeToGet;
                    customerSlotsFilled++;
                }
                availablePoopTypes.Remove(poopTypeToGet);
                if(maxForCustomer <= 0)
                    break;
            }
        }

        private List<PoopType> GetUsablePoopTypes()
        {
            List<PoopType> usablePoopTypes = new List<PoopType>();
            foreach(var key in _poopTypeNumerable)
            {
                PoopType poopType = (PoopType)key;
                if(GetIsPoopTypeUsable((PoopType)key))
                {
                    usablePoopTypes.Add(poopType);
                }
            }
            return usablePoopTypes;
        }

        protected virtual bool CanSpawn()
        {
            return GetUsablePoopTypeCount() > 0;
        }

        private bool IsPoopTypesValid(PoopType[] poopTypes)
        {
            for(int i = 0; i < poopTypes.Length; i++)
            {
                if(poopTypes[i] != PoopType.None)
                    return true;
            }
            return false;
        }

        protected int GetUsablePoopTypeCount()
        {
            int activePoopTypeCount = 0;
            foreach(var usablePoopType in _poopTypeNumerable)
            {
                PoopType poopType = (PoopType)usablePoopType;
                
                if(GetIsPoopTypeUsable(poopType))
                    activePoopTypeCount++;
            }
            return activePoopTypeCount;
        }

        protected virtual bool GetIsPoopTypeUsable(PoopType poopType)
        {
            return (PoopTypeActivityController.instance.GetIsPoopTypeActive(poopType)) && ScenePointManager.instance.GetPoopShowPlaceHasFreePosition(poopType);
        }
    }
}
