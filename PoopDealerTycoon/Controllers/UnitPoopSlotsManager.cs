using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class UnitPoopSlotsManager : PoopSlotsManager
    {
        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                poopSlot.SlotCleared += OnSlotCleared;
                poopSlot.SlotFilled += OnSlotFilled;
            }
        }

        private void UnregisterEvents()
        {
            foreach(PoopSlot poopSlot in _poopSlots)
            {
                poopSlot.SlotCleared -= OnSlotCleared;
                poopSlot.SlotFilled -= OnSlotFilled;
            }
        }

        public void ReorganizeSlots()
        {
            for(int i = 0; i < _poopSlots.Count - 1; i++)
            {
                if(!_poopSlots[i].IsFull && _poopSlots[i + 1].IsFull)
                {
                    MoveEveryoneBackFromIndex(i);
                    break;
                }
            }
            OnReorganizedSlots();
        }

        private void MoveEveryoneBackFromIndex(int index)
        {
            for(int i = index; i < _poopSlots.Count - 1; i++)
            {
                PoopSlot tempSlot = _poopSlots[i];
                SwapPositions(_poopSlots[i].transform, _poopSlots[i + 1].transform);
                _poopSlots[i] = _poopSlots[i + 1];
                _poopSlots[i + 1] = tempSlot;
            }
        }

        private void SwapPositions(Transform swapFirst, Transform swapSecond)
        {
            Vector3 tempLocalPos = swapFirst.localPosition;
            swapFirst.localPosition = swapSecond.localPosition;
            swapSecond.localPosition = tempLocalPos;
        }
        protected virtual void OnSlotFilled(PoopSlot poopSlot)
        {
            ReorganizeSlots();
            int slotIndex = GetIndexOfSlot(poopSlot);
            ArrangeNextSlotOffset(slotIndex);
        }

        protected virtual void OnSlotCleared(PoopSlot clearedSlot)
        {
            ReorganizeSlots();
        }

        private void OnReorganizedSlots()
        {
            for(int i = 0; i < _poopSlots.Count - 1; i++)
            {
                ArrangeNextSlotOffset(i);
            }
        }

        private int GetIndexOfSlot(PoopSlot targetPoopSlot)
        {
            for(int i = 0; i < _poopSlots.Count; i++)
            {
                if(targetPoopSlot == _poopSlots[i])
                    return i;
            }
            Debug.LogWarning("Poop slot not found");
            return 0;
        }

        private void ArrangeNextSlotOffset(int currentIndex)
        {
            if(currentIndex + 1 >= _poopSlots.Count)
                return;
            if(!_poopSlots[currentIndex].IsFull)
                return;

            _poopSlots[currentIndex + 1].transform.position = _poopSlots[currentIndex].GetPoopInSlot().GetTopOfPoop();
        }

    }
}