using System;
using System.Collections.Generic;
using Game.Utils.Abstractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Player
{
    public sealed class BulletFactory
        : ITickable, IDisposable
    {
        public BulletFactory(PlayerSettings playerSettings, BulletView prefab, Transform parent)
        {
            _playerSettings = playerSettings;
            _prefab = prefab;
            _parent = parent;
        }

        private readonly PlayerSettings _playerSettings;
        private readonly BulletView _prefab;
        private readonly Transform _parent; 
        
        private readonly ICollection<ITickable> _tickables = new List<ITickable>();
        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        public BulletModel Spawn(Transform from, BulletTarget target, int damage)
        {
            var model = new BulletModel(_playerSettings.Damage, _playerSettings.BulletSpeed);
            var view = Object.Instantiate(_prefab, _parent);
            
            var presenter = new BulletPresenter(model, view, target, from.position, damage);
            presenter.Initialize();

            _tickables.Add(presenter);
            _disposables.Add(presenter);

            return model;
        }

        public void Tick(float deltaTime)
        {
            var incoming = new List<ITickable>(_tickables);
            foreach (var tickable in incoming)
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