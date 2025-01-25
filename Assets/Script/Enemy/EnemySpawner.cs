using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Tier
{
    T1, T2, T3, T4, T5, T6, T7
}

public class EnemySpawner : MonoBehaviour, IPauseable
{
    [SerializeField] private bool firstCycle = true;

    [SerializeField] private Tier _actualTier;
    [SerializeField] private Tier[] _actualTiersAfterFirstCycle = new Tier[2];
    [SerializeField] private float _timeToNextTier;

    [SerializeField] private float _minSpawnTimer;
    [SerializeField] private float _maxSpawnTimer;

    [SerializeField] private float _minXSpawnPosition;
    [SerializeField] private float _maxXSpawnPosition;

    [SerializeField] private float _minYSpawnPosition;
    [SerializeField] private float _maxYSpawnPosition;

    private int _actualPlayerNumber = 0;

    [SerializeField] private EnemyTierListSO _enemyTierListSO;

    [SerializeField] private List<EnemyTierList> pool = new List<EnemyTierList>();
    [SerializeField] private int _maxPoolSize = 40;

    [SerializeField] private bool _isSpawning = false;

    [SerializeField] private bool _isInPause = false;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    void Awake()
    {
        EventManager.OnEnemyDeath += RemoveEnemyFromPool;
    }

    void Start()
    {
        TierUpdate().Forget();
        SpawnEnemies().Forget();
    }

    void Update()
    {
        if (pool.Count >= _maxPoolSize
            || _isInPause == true)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            _isSpawning = false;
        }
        else if (pool.Count < _maxPoolSize
            && _isInPause == false)
        {
            if (_isSpawning == false)
            {
                _isSpawning = true;
                SpawnEnemies().Forget();
                TierUpdate().Forget();
            }
        }
    }

    [BurstCompile]
    private async UniTask SpawnEnemies()
    {
        while (_isSpawning)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }

            _actualPlayerNumber = ControllerPlayersManager.Instance.Players.Count;

            var spawnTimer = Random.Range(_minSpawnTimer, _maxSpawnTimer);

            await UniTask.WaitForSeconds(spawnTimer, cancellationToken: _cancellationTokenSource.Token);

            var x = Random.Range(_minXSpawnPosition, _maxXSpawnPosition);
            var y = Random.Range(_minYSpawnPosition, _maxYSpawnPosition);

            if (firstCycle)
            {
                var ind = Random.Range(0, _actualPlayerNumber);
                var spawnPosition = new Vector3(ControllerPlayersManager.Instance.Players[ind].transform.position.x + x,
                                                ControllerPlayersManager.Instance.Players[ind].transform.position.y + y,
                                                0);
                pool.Add(new EnemyTierList { enemyObject = Instantiate(_enemyTierListSO.enemyTierList[(int)_actualTier].enemyObject, spawnPosition, Quaternion.identity), tier = _actualTier });
            }
            else
            {
                var ind = Random.Range(0, _actualPlayerNumber);
                var spawnPosition = new Vector3(ControllerPlayersManager.Instance.Players[ind].transform.position.x + x,
                                            ControllerPlayersManager.Instance.Players[ind].transform.position.y + y,
                                            0);
                pool.Add(new EnemyTierList { enemyObject = Instantiate(_enemyTierListSO.enemyTierList[(int)_actualTiersAfterFirstCycle[0]].enemyObject, spawnPosition, Quaternion.identity), tier = _actualTiersAfterFirstCycle[0] });

                ind = Random.Range(0, _actualPlayerNumber);
                spawnPosition = new Vector3(ControllerPlayersManager.Instance.Players[ind].transform.position.x + x,
                                            ControllerPlayersManager.Instance.Players[ind].transform.position.y + y,
                                            0);
                pool.Add(new EnemyTierList { enemyObject = Instantiate(_enemyTierListSO.enemyTierList[(int)_actualTiersAfterFirstCycle[1]].enemyObject, spawnPosition, Quaternion.identity), tier = _actualTiersAfterFirstCycle[1] });
            }
        }
    }

    [BurstCompile]
    private async UniTask TierUpdate()
    {
        while (_isSpawning)
        {
            await UniTask.WaitForSeconds(_timeToNextTier, cancellationToken: _cancellationTokenSource.Token);

            if (_cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }

            if (_actualTier == Tier.T7)
            {
                if (firstCycle)
                {
                    firstCycle = false;
                    _actualTiersAfterFirstCycle[0] = Tier.T7;
                    _actualTiersAfterFirstCycle[1] = Tier.T1;
                    TierUpdateAfterFirstCycle().Forget();
                }
            }
            else
            {
                _actualTier = (Tier)((int)_actualTier + 1);  // Increment correctly from T1 to T7
            }
        }
    }

    [BurstCompile]
    private async UniTask TierUpdateAfterFirstCycle()
    {
        await UniTask.WaitForSeconds(_timeToNextTier, cancellationToken: _cancellationTokenSource.Token);

        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            // Increment tiers correctly after first cycle
            if (_actualTiersAfterFirstCycle[0] == Tier.T7)
            {
                _actualTiersAfterFirstCycle[0] = Tier.T1;
                _actualTiersAfterFirstCycle[1] = Tier.T2; // Fix the increment to avoid skipping
            }
            else
            {
                _actualTiersAfterFirstCycle[0] = (Tier)((int)_actualTiersAfterFirstCycle[0] + 1);
                _actualTiersAfterFirstCycle[1] = (Tier)((int)_actualTiersAfterFirstCycle[1] + 1);
            }

            TierUpdateAfterFirstCycle().Forget();
        }
    }

    private void RemoveEnemyFromPool(GameObject enemy)
    {
        pool.Remove(pool.Find(x => x.enemyObject == enemy));
    }

    public void Pause()
    {
        _isInPause = true;
    }

    public void Unpause()
    {
        _isInPause = false;
    }
}
