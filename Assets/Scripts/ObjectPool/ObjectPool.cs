using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        private PooledObject prefab;
        private readonly List<PooledObject> availableObjects = new List<PooledObject>();

        public PooledObject GetObject()
        {
            PooledObject obj;

            if (availableObjects.Count - 1 >= 0)
            {
                int lastAvailableIndex = availableObjects.Count - 1;
                obj = availableObjects[lastAvailableIndex];
                availableObjects.RemoveAt(lastAvailableIndex);
                obj?.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate<PooledObject>(prefab);
                obj.transform.SetParent(transform, false);
                obj.Pool = this;
            }

            return obj;
        }

        public void AddObject(PooledObject obj)
        {
            obj.gameObject.SetActive(false);
            availableObjects.Add(obj);
        }

        public static ObjectPool GetPool(PooledObject prefab)
        {
            ObjectPool pool;
            GameObject obj = GameObject.Find(prefab.name + " Pool");

            if (obj)
            {
                pool = obj.GetComponent<ObjectPool>();
                if (pool)
                {
                    return pool;
                }
            }

            obj = new GameObject(prefab.name + " Pool")
            {
                transform =
                {
                    position = Vector3.zero
                }
            };

            DontDestroyOnLoad(obj);
            pool = obj.AddComponent<ObjectPool>();
            pool.prefab = prefab;
            return pool;
        }
    }
}