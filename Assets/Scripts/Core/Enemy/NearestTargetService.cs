using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Player;
using Game.Utils;
using UnityEngine;

namespace Game.Core.Enemy
{
    public sealed class NearestTargetService
    {
        private readonly ICollection<BulletTarget> _views = new List<BulletTarget>();

        public IDisposable RegisterEnemy(BulletTarget view)
        {
            _views.Add(view);

            var token = new CallbackDisposable(() => _views.Remove(view));
            return token;
        }
        
        public bool TryGetTarget(Vector2 position, out BulletTarget result)
        {
            var hasElements = _views.Any();
            if (!hasElements)
            {
                result = null;
                return false;
            }
            
            var ordered = _views.OrderBy(value => (value.Transform.position - (Vector3) position).sqrMagnitude);
            result = ordered.First();
            return true;
        }
    }
}