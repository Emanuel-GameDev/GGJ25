using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Tier
{
    T1, T2, T3, T4, T5, T6, T7
}
public class EnemySpawner : MonoBehaviour
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

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    
    void Start()
    {
        TierUpdate().Forget();
        SpawnEnemies().Forget();
    }

    [BurstCompile]
    private async UniTask SpawnEnemies()
    {
        while(true)
        {
            _actualPlayerNumber = ControllerPlayersManager.Instance.Players.Count;

            var spawnTimer = Random.Range(_minSpawnTimer, _maxSpawnTimer);

            await UniTask.WaitForSeconds(spawnTimer);

            var x = Random.Range(_minXSpawnPosition, _maxXSpawnPosition);
            var y = Random.Range(_minYSpawnPosition, _maxYSpawnPosition);

            if(firstCycle)
            {
                var ind = Random.Range(0, _actualPlayerNumber);
                var spawnPosition = new Vector3(ControllerPlayersManager.Instance.Players[ind].transform.position.x + x, 
                                                ControllerPlayersManager.Instance.Players[ind].transform.position.y + y, 
                                                0);
                Instantiate(_enemyTierListSO.enemyTierList[(int)_actualTier].enemyPrefab, spawnPosition, Quaternion.identity); 
            }
            else
            {
                var ind = Random.Range(0, _actualPlayerNumber);
                var spawnPosition = new Vector3(ControllerPlayersManager.Instance.Players[ind].transform.position.x + x, 
                                            ControllerPlayersManager.Instance.Players[ind].transform.position.y + y, 
                                            0);
                Instantiate(_enemyTierListSO.enemyTierList[(int)_actualTiersAfterFirstCycle[0]].enemyPrefab, spawnPosition, Quaternion.identity); 
                
                ind = Random.Range(0, _actualPlayerNumber);
                spawnPosition = new Vector3(ControllerPlayersManager.Instance.Players[ind].transform.position.x + x, 
                                            ControllerPlayersManager.Instance.Players[ind].transform.position.y + y, 
                                            0);
                Instantiate(_enemyTierListSO.enemyTierList[(int)_actualTiersAfterFirstCycle[1]].enemyPrefab, spawnPosition, Quaternion.identity); 
            }
        }
    }

    [BurstCompile]
    private async UniTask TierUpdate()
    {
        await UniTask.WaitForSeconds(_timeToNextTier, cancellationToken: _cancellationTokenSource.Token);
        
        if(_actualTier == Tier.T7)
        {
            if(firstCycle)
            {
                firstCycle = false;
                _actualTiersAfterFirstCycle[0] = Tier.T7;
                _actualTiersAfterFirstCycle[1] = Tier.T1;
                TierUpdateAfterFirstCycle().Forget();
            }
        }
        else
        {
            _actualTier++;
            TierUpdate().Forget();
        }
    }

    [BurstCompile]
    private async UniTask TierUpdateAfterFirstCycle()
    {
        await UniTask.WaitForSeconds(_timeToNextTier, cancellationToken: _cancellationTokenSource.Token);

        if(_actualTiersAfterFirstCycle[0] == Tier.T7)
        {
            _actualTiersAfterFirstCycle[0] = Tier.T1;
            _actualTiersAfterFirstCycle[1] = Tier.T7;
        }
        else
        {
            _actualTiersAfterFirstCycle[0]++;
            _actualTiersAfterFirstCycle[1]++;
        }

        TierUpdateAfterFirstCycle().Forget();
    }
}
