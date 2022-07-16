using System;
using Enums;
using UnityEngine;

namespace DataModel
{
    [Serializable]
    public struct RacketModel
    {
        [SerializeField] private PlayerSide side;
        [SerializeField] private Transform transform;
        
        public PlayerSide Side
        {
            get => side;
            private set => side = value;
        }
        
        public Transform Transform
        {
            get => transform;
            private set => transform = value;
        }

        public RacketModel(PlayerSide side, Transform transform)
        {
            this.side = side;
            this.transform = transform;
        }
    }
}