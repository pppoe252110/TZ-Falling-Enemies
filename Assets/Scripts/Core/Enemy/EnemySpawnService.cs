using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Utils;
using Game.Utils.Abstractions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core.Enemy
{
    public sealed class EnemySpawnService
        : IInitializable, IDisposable
    {
        public EnemySpawnService(EnemySettings settings, EnemyFactory factory, IEnumerable<Vector2> spawnPoints, MonoBehaviour coroutineRunner)
        {
            _settings = settings;
            _factory = factory;
            _spawnPoints = spawnPoints.ToList();
            _coroutineRunner = coroutineRunner;
        }

        // True - спавнит врага моментально в начале игры. False - выжидает таймаут, указанный в настройках
        private const bool SpawnInstantly = true;

        private readonly EnemySettings _settings;
        private readonly EnemyFactory _factory;
        private readonly IReadOnlyList<Vector2> _spawnPoints;
        private readonly MonoBehaviour _coroutineRunner; // Есть более элегантные решения, но они бы значительно раздули кодовую базу

        private readonly ICollection<EnemyModel> _activeEnemies = new List<EnemyModel>();

        private Coroutine _coroutine;
        
        public void Initialize()
        {
            _coroutine = _coroutineRunner.StartCoroutine(SpawnCycleCoroutine());
        }

        public void Dispose()
        {
            _coroutineRunner.StopCoroutine(_coroutine);
        }

        private void Spawn()
        {
            var randomIndex = Random.Range(0, _spawnPoints.Count);
            var point = _spawnPoints[randomIndex];
            
            var model = _factory.Spawn(point);
            _activeEnemies.Add(model);
            
            model.OnDeath += HandleDeath;

            void HandleDeath()
            {
                model.OnDeath -= HandleDeath;
                _activeEnemies.Remove(model);
            }
        }

        private IEnumerator SpawnCycleCoroutine()
        {
            if (SpawnInstantly)
            {
                Spawn();
            }

            while (true)
            {
                yield return new WaitForSeconds(_settings.SpawnDelayRange.GetRandomValue());
                Spawn();
            }
        }
    }
}