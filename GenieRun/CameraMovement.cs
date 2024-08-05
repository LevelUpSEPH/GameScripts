using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : CinemachineExtension
{

    [SerializeField] private float _cameraLeftLimit = 0.5f;
    [SerializeField] private float _cameraRightLimit = 1.5f;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(stage == CinemachineCore.Stage.Body){
            var pos = state.RawPosition;
            if(pos.x < _cameraLeftLimit)
                pos.x = _cameraLeftLimit;
            if(pos.x > _cameraRightLimit)
                pos.x = _cameraRightLimit;
            state.RawPosition = pos;
        }
    }

}
