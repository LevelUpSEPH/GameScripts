using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class EventCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _eventCamera;
        private void Start()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            SpecialCustomerBehaviour.SpecialCustomerSpawned += OnSpecialCustomerSpawned;

            UpgradeZone.UpgradeZoneFirstEnable += OnCameraTargetSpawned;
            ZoneBehaviour.ZoneActivatedFirstTime += OnCameraTargetSpawned;
        }

        private void UnregisterEvents()
        {
            SpecialCustomerBehaviour.SpecialCustomerSpawned -= OnSpecialCustomerSpawned;
            
            UpgradeZone.UpgradeZoneFirstEnable -= OnCameraTargetSpawned;
            ZoneBehaviour.ZoneActivatedFirstTime -= OnCameraTargetSpawned;
        }

        private void OnSpecialCustomerSpawned(SpecialCustomerBehaviour specialCustomer)
        {
            OnCameraTargetSpawned(specialCustomer.transform);
        }

        private void OnCameraTargetSpawned(Transform targetTransform)
        {
            _eventCamera.Follow = targetTransform;
            _eventCamera.LookAt = targetTransform;
            StartCoroutine(EventCameraLifetime());
        }

        private IEnumerator EventCameraLifetime()
        {
            _eventCamera.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            _eventCamera.gameObject.SetActive(false);
        }
    }
}