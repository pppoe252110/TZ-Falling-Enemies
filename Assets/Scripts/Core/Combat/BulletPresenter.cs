using System;
using Game.Utils;
using Game.Utils.Abstractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Player
{
    public sealed class BulletPresenter
        : IInitializable, ITickable, IDisposable
    {
        public BulletPresenter
        (
            BulletModel model, 
            BulletView view, 
            BulletTarget target,
            Vector2 position,
            int damage
        )
        {
            _model = model;
            _view = view;
            _target = target;
            
            _position = position;
            _damage = damage;
        }

        private const float Tolerance = 0.15f;

        private readonly BulletModel _model;
        private readonly BulletView _view;
        private readonly BulletTarget _target;
        
        private readonly Vector2 _position;
        private readonly int _damage;

        private bool _isTargetDestroyed;
        
        public void Initialize()
        {
            _view.transform.position = _position;
            _target.Damageable.OnDeath += Dispose;
        }

        public void Tick(float deltaTime)
        {
            if (_isTargetDestroyed)
            {
                return;
            }

            var delta = _target.Transform.position - _view.transform.position;
            _view.Move(delta.normalized * _model.Speed * deltaTime);

            var distance = delta.magnitude;
            if (distance < Tolerance)
            {
                var isKill = _target.Damageable.Hit(_damage);
                if (isKill)
                {
                    StaticEventBus<KillSignal>.Self.Shoot(new KillSignal());
                }
                
                Dispose();
            }
        }

        public void Dispose()
        {
            if (_isTargetDestroyed)
            {
                return;
            }
            
            _target.Damageable.OnDeath -= Dispose;
            
            Object.Destroy(_view.gameObject);
            _isTargetDestroyed = true;
        }
    }
}
