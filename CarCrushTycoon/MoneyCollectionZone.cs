using System.Collections;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class MoneyCollectionZone : MonoBehaviour
    {
        [SerializeField] private MoneyStackingArea _targetMoneyStackingArea;
        private bool _isCollectMoneyActive = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(_isCollectMoneyActive)
                    return;
                StartCoroutine(CollectMoney(other.transform));
                _isCollectMoneyActive = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                StopAllCoroutines();
                _isCollectMoneyActive = false;
            }
        }

        private IEnumerator CollectMoney(Transform playerTransform)
        {
            while(true)
            {
                _targetMoneyStackingArea.CollectMoney(playerTransform.transform);
                yield return new WaitForSeconds(1);
            }
        }
    }
}