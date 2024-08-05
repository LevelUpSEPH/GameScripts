using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class CarPool : Singleton<CarPool>
    {
         [System.Serializable]
        public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

        [SerializeField] private List<Pool> pools;
        protected Dictionary<string, Queue<GameObject>> poolDictionary;

        void Start() {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools) {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++) {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag)) 
            {
                Debug.LogWarning("Pool with tag: " + tag + " does not exist");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            CarController carToSpawn = objectToSpawn.GetComponentInChildren<CarController>();

            carToSpawn.SetTransform(position, rotation);

            objectToSpawn.SetActive(true);

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }
}