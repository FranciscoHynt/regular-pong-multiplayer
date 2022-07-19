using System;
using UnityEngine;

namespace ObjectPool
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool Pool { get; set; }

        [NonSerialized]
        private ObjectPool poolInstanceForPrefab;

        public void ReturnToPool()
        {
            if (Pool)
            {
                Pool.AddObject(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public T GetPooledInstance<T>() where T : PooledObject
        {
            if (!poolInstanceForPrefab)
            {
                poolInstanceForPrefab = ObjectPool.GetPool(this);
            }

            return (T)poolInstanceForPrefab.GetObject();
        }
    }
}