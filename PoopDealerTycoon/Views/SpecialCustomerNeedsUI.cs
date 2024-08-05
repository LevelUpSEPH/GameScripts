using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chameleon.Game.ArcadeIdle.Units
{
    public class SpecialCustomerNeedsUI : NeedsUI
    {
        [SerializeField] private SpecialCustomerBehaviour _specialCustomer;
        [SerializeField] private Transform _wantedImagesParent;
        [SerializeField] private List<GameObject> _needItemObjects = new List<GameObject>();
        private List<NeedItem> _needItems = new List<NeedItem>();

        protected override void RegisterEvents()
        {
            SpecialCustomerBehaviour.SpecialCustomerArrived += InitializeNeeds;
            SpecialCustomerBehaviour.SpecialCustomerLeft += OnCustomerLeft;
            _specialCustomer.ReceivedPoop += OnReceivedPoop;
        }

        protected override void UnregisterEvents()
        {
            SpecialCustomerBehaviour.SpecialCustomerArrived -= InitializeNeeds;
            SpecialCustomerBehaviour.SpecialCustomerLeft -= OnCustomerLeft;
            _specialCustomer.ReceivedPoop -= OnReceivedPoop;
        }

        private void OnReceivedPoop(PoopType poopType)
        {
            foreach(NeedItem needItem in _needItems)
            {
                if(needItem.GetNeededType() == poopType)
                {
                    needItem.gameObject.SetActive(false);
                    _needItems.Remove(needItem);
                    return;
                }
            }
            Debug.Log("Poop type does not exist in neededList?");
        }

        private void OnCustomerLeft()
        {
            _needItems.Clear();
            SetVisualsActive(false);
        }

        private void InitializeNeeds()
        {
            SetVisualsActive(true);
            foreach(PoopType poopType in _specialCustomer.GetNeeds())
            {
                GameObject needItemObject = ActivateNeedItem();
                if(needItemObject.transform.parent != _wantedImagesParent)
                {
                    needItemObject.transform.parent = _wantedImagesParent;
                    needItemObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
                NeedItem needItem = needItemObject.GetComponent<NeedItem>();
                needItem.SetNeedType(poopType);
                _needItems.Add(needItem);   
            }
        }

        protected override void UpdateWantedImage()
        {
            throw new System.NotImplementedException();
        }

        private GameObject ActivateNeedItem()
        {
            foreach(GameObject needObject in _needItemObjects)
            {
                if(!needObject.activeInHierarchy)
                {
                    needObject.SetActive(true);
                    return needObject;
                }
            }
            return null;
        }
    }
}