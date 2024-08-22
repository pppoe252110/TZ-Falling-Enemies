using System;
using Game.Core.Abstractions;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class BulletTarget
    {
        public BulletTarget(Transform transform, IDamageable damageable)
        {
            Transform = transform;
            Damageable = damageable;
        } 

        public Transform Transform
        {
            get;
        }

        public IDamageable Damageable
        {
            get;
        }
    }
}