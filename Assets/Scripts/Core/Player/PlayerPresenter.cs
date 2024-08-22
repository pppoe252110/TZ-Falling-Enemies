using System;
using System.Collections;
using Game.Core.Enemy;
using Game.Input;
using Game.Utils;
using Game.Utils.Abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.Player
{
    public sealed class PlayerPresenter: IInitializable, ITickable, IDisposable
    {
        public PlayerPresenter
        (
            PlayerSettings settings, 
            PlayerModel model, 
            PlayerView view,
            PlayerHealthView healthView, 
            BulletFactory bulletFactory,
            NearestTargetService nearestTargetService,
            MonoBehaviour coroutineRunner
        )
        {
            _settings = settings;
            _model = model;
            _view = view;
            _healthView = healthView;
            _bulletFactory = bulletFactory;
            _nearestTargetService = nearestTargetService;
            _coroutineRunner = coroutineRunner;
        }

        private readonly PlayerSettings _settings;
        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        private readonly PlayerHealthView _healthView;
        private readonly BulletFactory _bulletFactory;
        private readonly NearestTargetService _nearestTargetService;
        private readonly MonoBehaviour _coroutineRunner;
        
        private readonly PlayerActions _actions = new PlayerActions();

        private Coroutine _coroutine;
        private IDisposable _damageToken;
        
        public void Initialize()
        {
            _damageToken = StaticEventBus<DamageInfo>.Self.Subscribe(HandleDamage);
            
            _coroutine = _coroutineRunner.StartCoroutine(ShootCoroutine());
            
            _actions.Enable();
            _model.OnHealthChanged += HandleHealthChanged;
        }

        public void Tick(float deltaTime)
        {
            if (_actions.Keyboard.Movement.IsInProgress())
            {
                var delta = _actions.Keyboard.Movement.ReadValue<Vector2>().normalized * _settings.MovementSpeed;
                _view.Move(delta * deltaTime, _settings.MovementRect);
            }
        }

        public void Dispose()
        {
            _damageToken.Dispose();
            
            _actions.Disable();
            _model.OnHealthChanged -= HandleHealthChanged;
            
            _coroutineRunner.StopCoroutine(_coroutine);
        }

        private void HandleHealthChanged(int health)
        {
            _healthView.SetHealth(health);
        }

        private void HandleDamage(DamageInfo info)
        {
            var isDead = _model.Hit(info.Amount);
            if (isDead)
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_settings.ReloadDuration);

                var hasEnemies = _nearestTargetService.TryGetTarget(_view.transform.position, out var target);
                if (hasEnemies)
                {
                    if(Vector3.Distance(target.Transform.position, _view.transform.position) < _settings.AttackDistance)
                    {
                        _bulletFactory.Spawn(_view.transform, target, _settings.Damage);
                    }
                }
            }
        }
    }
}