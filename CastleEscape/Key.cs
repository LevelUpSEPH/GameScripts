using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Key : MonoBehaviour
{
    public static event Action<KeyType> AnyKeyAcquired;

    public enum KeyType{
        OrangeKey,
        GreenKey,
        RedKey,
        BlueKey,
        PurpleKey
    }

    [SerializeField] private KeyType _keyType = KeyType.OrangeKey;

    public KeyType GetKeyType(){
        return _keyType;
    }

    public void CollectedKey(){
        AnyKeyAcquired?.Invoke(_keyType);
        Destroy(gameObject);
    }

}
