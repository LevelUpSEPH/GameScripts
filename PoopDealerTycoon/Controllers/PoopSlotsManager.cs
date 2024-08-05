using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopSlotsManager : MonoBehaviour // first in last out
    {
        [SerializeField] protected List<PoopSlot> _poopSlots = new List<PoopSlot>();

        public bool TryGetEmptyPoopSlot(out PoopSlot emptySlot)
        {
            emptySlot = null;
            for(int i = 0; i < _poopSlots.Count; i++)
            {
                if(_poopSlots[i].IsFull || !_poopSlots[i].GetIsSlotActive() || !_poopSlots[i].GetIsSlotAvailable())
                {
                    continue;
                }
                else
                {
                    emptySlot = _poopSlots[i];
                    return true;
                }
            }
            return false;
        }

        public bool TryGetFullSlot(out PoopSlot fullSlot)
        {
            for(int i = _poopSlots.Count - 1; i >= 0; i--)
            {
                if(_poopSlots[i].IsFull){
                    fullSlot = _poopSlots[i];
                    return true;
                }
            }
            fullSlot = null;
            return false;
        }

        public bool TryGetEmptySlotWithType(out PoopSlot poopSlot, PoopType poopType)
        {
            for(int i = 0; i < _poopSlots.Count; i++)
            {
                if(_poopSlots[i].IsFull || !_poopSlots[i].GetIsSlotActive())
                    continue;
                if(_poopSlots[i].TryGetComponent<PoopSlotWithType>(out PoopSlotWithType poopSlotWithType))
                {
                    if(poopType == poopSlotWithType.GetAcceptedPoopType())
                    {
                        poopSlot = poopSlotWithType;
                        return true;
                    }
                }
            }
            poopSlot = null;
            return false;
        }

        public bool TryGetFullSlotWithType(out PoopSlot poopSlot, PoopType poopType)
        {
            for(int i = _poopSlots.Count - 1; i >= 0; i--)
            {
                if(!_poopSlots[i].IsFull || !_poopSlots[i].GetIsSlotActive())
                    continue;
                PoopBase poop = _poopSlots[i].GetPoopInSlot();
                if(poop.GetPoopType() == poopType)
                {
                    poopSlot = _poopSlots[i];
                    return true;
                }
            }
            poopSlot = null;
            return false;
        }

        public bool TryGetSlotOfPoop(out PoopSlot poopSlot, PoopBase poop)
        {
            foreach(PoopSlot iteratingSlot in _poopSlots)
            {
                if(iteratingSlot.GetPoopInSlot() == poop)
                {
                    poopSlot = iteratingSlot;
                    return true;
                }
            }
            poopSlot = null;
            return false;
        }


        public bool GetHasEmptySlotWithType(PoopType poopType)
        {
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                if(poopSlot.TryGetComponent<PoopSlotWithType>(out PoopSlotWithType poopSlotWithType))
                {
                    if(poopSlotWithType.GetAcceptedPoopType() != poopType)
                        continue;
                    if(poopSlotWithType.IsFull || !poopSlotWithType.GetIsSlotActive())
                        continue;
                    else
                        return true;
                }
            }
            return false;
        }

        public List<PoopBase> GetPoops()
        {
            List<PoopBase> poops = new List<PoopBase>();
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                if(poopSlot.TryGetPoopInSlot(out PoopBase poop))
                    poops.Add(poop);
            }
            return poops;
        }

        public void ActivateSlots(int slotCountToActivate)
        {
            int activeCount = GetActiveSlotCount();
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                if(activeCount >= slotCountToActivate)
                    break;
                if(poopSlot.GetIsSlotActive())
                    continue;
                else
                {
                    poopSlot.SetSlotActive(true);
                    activeCount++;
                }
            }
            OnActiveSlotCountChanged();
        }

        public PoopSlot GetSlotAtIndex(int index)
        {
            return _poopSlots[index];
        }

        public int GetFullSlotCount()
        {
            int fullSlotCount = 0;
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                if(poopSlot.IsFull)
                    fullSlotCount++;
            }
            return fullSlotCount;
        }

        public int GetSlotCount()
        {
            return _poopSlots.Count;
        }

        public int GetActiveSlotCount()
        {
            int activeSlotCount = 0;
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                if(poopSlot.GetIsSlotActive())
                    activeSlotCount++;
            }
            return activeSlotCount;
        }

        public bool GetHasPoop()
        {
            for(int i = _poopSlots.Count - 1; i >= 0; i--)
            {
                if(_poopSlots[i].IsFull)
                    return true;
            }
            return false;
        }

        public bool GetIsFull()
        {
            for(int i = _poopSlots.Count - 1; i >= 0; i--)
            {
                if(!_poopSlots[i].GetIsSlotActive())
                    continue;
                if(_poopSlots[i].IsFull)
                    continue;
                if(!_poopSlots[i].GetIsSlotAvailable())
                    continue;
                else
                    return false;
            }
            return true;
        }

        public bool GetHasEmptyActiveSlot()
        {
            for(int i = 0; i < _poopSlots.Count; i++)
            {
                if(_poopSlots[i].IsFull)
                    continue;
                if(!_poopSlots[i].GetIsSlotActive())
                    continue;
                return true;
            }
            return false;
        }

        public void ResetSlots()
        {
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                if(poopSlot.TryGetPoopInSlot(out PoopBase poop))
                {
                    poop.DisablePoop();
                }
                poopSlot.ClearSlot();
                poopSlot.SetSlotActive(false);
            }
        }

        public void DisableSlots()
        {
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                poopSlot.ClearSlot();
                poopSlot.SetSlotActive(false);
            }
        }

        protected virtual void OnActiveSlotCountChanged()
        {

        }
    }
}
