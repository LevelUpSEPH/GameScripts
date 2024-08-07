using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithKey : Door
{
    [SerializeField] private Key.KeyType _lockType = Key.KeyType.OrangeKey;

    public bool TryToUnlockWith(Key.KeyType keyType){
        if(keyType == _lockType){
            UnlockDoor();
            return true;
        }
        else
            return false;
    }
    
}
