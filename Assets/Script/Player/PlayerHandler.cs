using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : Player
{
    #region Vars

    #region Weapons

    [SerializeField] List<BaseWeapon> weaponsEquipped;

    [SerializeField]
    private GameObject weaponContainerObj;

    #endregion

    #region Passives
    // Lista di passive
    #endregion

    #endregion

    #region UnityFunctions

    private void Update()
    {
        if (isShooting)
        {
            ShootAll();
        }
    }


    #endregion

    #region Weapons

    private void ShootAll()
    {
        foreach (BaseWeapon weapon in weaponsEquipped)
        {
            weapon.Shoot();
        }
    }

    public List<BaseWeapon> GetEquippedWeapons()
    {
        return weaponsEquipped;
    }

    public void EquipWeapon(GameObject weaponToEquipObj)
    {
        weaponToEquipObj.transform.parent = weaponContainerObj.transform;
        weaponToEquipObj.transform.localPosition = Vector3.zero;
        weaponsEquipped.Add(weaponToEquipObj.GetComponent<BaseWeapon>());    
    }

    public void UnEquipWeapon(BaseWeapon weapon)
    {
        // Non si usa per ora ma non si sa mai
    }

    #endregion
}
