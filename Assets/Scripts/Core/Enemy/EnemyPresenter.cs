using System;
using Game.Core.Player;
using Game.Utils;
using Game.Utils.Abstractions;
using Object = UnityEngine.Object;

namespace Game.Core.Enemy
{
    public sealed class EnemyPresenter
        : IInitializable, ITickable, IDisposable
    {
        public EnemyPresenter(EnemySettings settings, EnemyModel model, EnemyView view)
        {
            _settings = settings;
            _model = model;
            _view = view;

            _speed = _settings.SpeedRange.GetRandomValue();
        }

        private readonly EnemySettings _settings;
        private readonly EnemyModel _model;
        private readonly EnemyView _view;

        private readonly float _speed;

        private bool _isFinished;
        
        public void Initialize()
        {
            _model.OnDeath += HandleDeath;
        }

        public void Tick(float deltaTime)
        {
            if (_isFinished)
            {
                return;
            }
            
            _view.Move(_speed * deltaTime);

            if (_view.transform.position.y <= _settings.FinishLine)
            {
                HandleFinish();
            }
        }

        public void Dispose()
        {
            _model.OnDeath -= HandleDeath;
            
            _isFinished = true;
        }

        private void HandleFinish()
        {
            StaticEventBus<DamageInfo>.Self.Shoot(new DamageInfo(_settings.Damage));
            _model.Hit(_model.Health);

            _isFinished = true;
        }

        private void HandleDeath()
        {
            Object.Destroy(_view.gameObject);
            
            _isFinished = true;
        }
    }
}