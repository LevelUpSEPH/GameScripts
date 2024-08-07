using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDoorMotor : MonoBehaviour
{
    private HingeJoint _hingeJoint;
    private JointMotor _jointMotor;
    private float _angle;

    
    private void Start(){
        _hingeJoint = GetComponent<HingeJoint>();
        _jointMotor = _hingeJoint.motor;
    }

    private void Update(){
         _angle = _hingeJoint.angle;
         _jointMotor.force = 0f;
        _jointMotor.targetVelocity = -_angle * 5;
        _hingeJoint.motor = _jointMotor;
    }

}
