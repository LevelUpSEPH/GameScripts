using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected BoxCollider _doorBlock;
    protected virtual void UnlockDoor(){
        Destroy(_doorBlock);
    }
}
