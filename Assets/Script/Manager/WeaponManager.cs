using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponDatabase", menuName = "Weapon Database", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public List<GameObject> weaponDatabase;
}

public class WeaponManager : MonoBehaviour
{
    WeaponDatabase database;

    public static WeaponManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerLevelUp += EquipRandomWeapon;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerLevelUp -= EquipRandomWeapon;
    }

    public void EquipRandomWeapon(int level, PlayerLevelManager levelManager)
    {
        if (database.weaponDatabase.Count > 0)
        {
            int randomIndex = Random.Range(0, database.weaponDatabase.Count);
            GameObject randomWeaponPrefab = database.weaponDatabase[randomIndex];

            // Istanzia l'arma
            GameObject instantiatedWeapon = Instantiate(randomWeaponPrefab);

            // Aggiungi l'arma istanziata alla lista delle armi equipaggiate di PlayerHandler
            levelManager._playerHandler.EquipWeapon(instantiatedWeapon);

            Debug.Log("Equipped weapon: " + randomWeaponPrefab.name);
        }
        else
        {
            Debug.LogWarning("Weapon database is empty!");
        }
    }

}
