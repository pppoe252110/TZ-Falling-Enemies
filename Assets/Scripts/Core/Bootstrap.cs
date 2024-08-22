using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Enemy;
using Game.Core.Player;
using Game.Utils;
using Game.Utils.Abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    [AddComponentMenu("Game/Bootstrapper")]
    public sealed class Bootstrap: MonoBehaviour
    {
        [SerializeField] private GameSettings settings;
        
        [Header("Combat")]
        [SerializeField] private BulletView bulletPrefab;
        [SerializeField] private Transform bulletsParent;
                
        [Header("Enemy")] 
        [SerializeField] private EnemyView enemyPrefab;
        [SerializeField] private List<Transform> enemySpawnPoints;
        [SerializeField] private Transform enemyParent;
        
        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerHealthView playerHealthView;

        private readonly ICollection<ITickable> _tickables = new List<ITickable>();
        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        private int _kills;
        private int _killsToWin;
        
        private IDisposable _killToken;

        private void HandleKill(KillSignal _)
        {
            _kills++;
         
            Debug.Log($"Убийства: {_kills}/{_killsToWin}");
            
            if (_kills >= _killsToWin)
            {
                Dispose();
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }

        private void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            
            _tickables.Clear();
            _disposables.Clear();

            _kills = 0;
            _killToken.Dispose();
        }

        private void OnEnable()
        {
            var bulletFactory = new BulletFactory(settings.PlayerSettings, bulletPrefab, bulletsParent);
            var nearestEnemyService = new NearestTargetService();

            var playerModel = new PlayerModel(settings.PlayerSettings.Health);
            var playerPresenter = new PlayerPresenter(settings.PlayerSettings, playerModel, playerView, playerHealthView, bulletFactory, nearestEnemyService, this);
            playerPresenter.Initialize();
            
            playerHealthView.SetHealth(playerModel.Health);

            var enemyFactory = new EnemyFactory(settings.EnemySettings, enemyPrefab, enemyParent, nearestEnemyService);
            var enemySpawnService = new EnemySpawnService(settings.EnemySettings, enemyFactory, enemySpawnPoints.Select(value => (Vector2) value.position), this);
            enemySpawnService.Initialize();

            _tickables.Add(bulletFactory);
            _tickables.Add(playerPresenter);
            _tickables.Add(enemyFactory);
            
            _disposables.Add(bulletFactory);
            _disposables.Add(enemyFactory);
            _disposables.Add(enemySpawnService);
            _disposables.Add(playerPresenter);

            _killsToWin = settings.KillsToWin.GetRandomValue();
            _killToken = StaticEventBus<KillSignal>.Self.Subscribe(HandleKill);
        }

        private void Update()
        {
            foreach (var tickable in _tickables)
            {
                tickable.Tick(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
