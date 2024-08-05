using UnityEngine;
using Cinemachine;

namespace Chameleon.Game.Scripts.Controller
{
    public class CameraPositionSetter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

        private float _referenceAspectRatio = 9f / 16f;

        public void Start()
        {
            AdjustCameraSize();
        }

        public void AdjustCameraSize()
        {
            if (cinemachineCamera != null)
            {
                float currentAspectRatio = (float)Screen.width / Screen.height;

                cinemachineCamera.m_Lens.OrthographicSize = (_referenceAspectRatio / (currentAspectRatio)) * cinemachineCamera.m_Lens.OrthographicSize;
            }
        }
    }
}