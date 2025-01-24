using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    public int initialPoolSize = 10;

    private List<EnemyTierList> pool = new List<EnemyTierList>();

}
