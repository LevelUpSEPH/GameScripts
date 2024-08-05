using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour, ICollectable
{
    void ICollectable.Collect()
    {
        InLevelController.instance.IncreaseEarnedCoin();
        ObjectPool.instance.SpawnFromPool("GlitterExplosion", transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
