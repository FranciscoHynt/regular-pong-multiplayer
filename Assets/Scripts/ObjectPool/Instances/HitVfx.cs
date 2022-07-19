using UnityEngine;
using Utils;

namespace ObjectPool.Instances
{
    public class HitVfx : PooledObject
    {
        private float timeToDisable;
        private ParticleSystem vfxParticleSystem;

        private void Awake()
        {
            vfxParticleSystem = GetComponent<ParticleSystem>();
            timeToDisable = vfxParticleSystem.main.duration;
        }

        private void OnEnable()
        {
            this.CallWithDelay(ReturnToPool, timeToDisable);
        }
    }
}