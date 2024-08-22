using System;
using System.Collections.Generic;
using Game.Core.Player;
using Game.Utils;
using Game.Utils.Abstractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Enemy
{
    public sealed class EnemyFactory
        : ITickable, IDisposable
    {
        public EnemyFactory(EnemySettings settings, EnemyView prefab, Transform parent, NearestTargetService nearestTargetService)
        {
            _settings = settings;
            _prefab = prefab;
            _parent = parent;
            _nearestTargetService = nearestTargetService;
        }

        private readonly EnemySettings _settings;
        private readonly EnemyView _prefab;
        private readonly Transform _parent;
        private readonly NearestTargetService _nearestTargetService;

        private readonly ICollection<ITickable> _tickables = new List<ITickable>();
        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        public EnemyModel Spawn(Vector2 position)
        {
            var view = Object.Instantiate(_prefab, position, _prefab.transform.rotation, _parent);
            var model = new EnemyModel(_settings.Health);
            
            var token = _nearestTargetService.RegisterEnemy(new BulletTarget(view.transform, model));
            model.OnDeath += HandleDeath;
            
            var presenter = new EnemyPresenter(_settings, model, view);
            presenter.Initialize();

            _tickables.Add(presenter);
            _disposables.Add(presenter);
            
            return model;

            void HandleDeath()
            {
                model.OnDeath -= HandleDeath;
                token.Dispose();
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (var tickable in _tickables)
            {
                tickable.Tick(deltaTime);
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _tickables.Clear();
            _disposables.Clear();
        }
    }
}