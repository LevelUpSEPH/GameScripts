using System.Collections;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopDropper : MonoBehaviour
    {
        [SerializeField] protected Transform _poopInitialPos;
        [SerializeField] protected PoopType _productPoopType;
        [SerializeField] protected UnorganizedPoopStockPlace _droppedItemsStock;
        private float _poopCooldown = 1f;
        protected bool _isAnimating = false;

        protected virtual void Start()
        {
            StartCoroutine(DropPoopRoutine());
        }
        
        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }

        protected virtual bool CanDropPoop()
        {
            return _droppedItemsStock.GetCanReceivePoop() && !_isAnimating;
        }

        private IEnumerator DropPoopRoutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(_poopCooldown);
                if(!CanDropPoop())
                    continue;
                DropPoop();
            }
        }

        protected virtual void DropPoop()
        {
            if(_isAnimating)
                return;
            _isAnimating = true;
            StartCoroutine(SpawnPoopAndWait());

            IEnumerator SpawnPoopAndWait()
            {
                string poopTypeString = _productPoopType.ToString();
                GameObject poop = ObjectPool.instance.SpawnFromPool(poopTypeString, _poopInitialPos.position, Quaternion.identity);
                _droppedItemsStock.IncrementWaitingPoopAmount();
                yield return new WaitForSeconds(.5f);
                _isAnimating = false;
            }
        }
    }
}
