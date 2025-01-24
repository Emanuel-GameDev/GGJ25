using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewWeaponDatabase", menuName = "Weapon Database", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public List<GameObject> weaponDatabase;
}

