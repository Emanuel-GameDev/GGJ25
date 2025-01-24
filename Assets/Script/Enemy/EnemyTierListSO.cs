using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTierListSO", menuName = "ScriptableObjects/EnemyTierListSO")]
public class EnemyTierListSO : ScriptableObject
{
    public List<EnemyTierList> enemyTierList;
}

[Serializable]
public struct EnemyTierList
{
    public Tier tier;
    public GameObject enemyObject;
}
