using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : Player
{
    #region Vars

    #region Weapons

    [SerializeField] List<BaseWeapon> weaponsEquipped;

    [SerializeField]
    protected float _fireRate = .3f;

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
            Debug.Log("shooting in update handler pt2");
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

    public void EquipWeapon(GameObject weaponToEquipObj)
    {
        weaponToEquipObj.transform.parent = weaponContainerObj.transform;
        weaponToEquipObj.transform.localPosition = Vector3.zero;    

        BaseWeapon weaponToEquip = weaponToEquipObj.GetComponent<BaseWeapon>();
        weaponToEquip.playerHandler = this;

        // questo potrebbe dare errore nel caso BaseWeapon andasse a cercare il primo base weapon uguale
        BaseWeapon existingWeapon = weaponsEquipped.Find(w => w.GetType() == weaponToEquip.GetType());

        if (existingWeapon != null)
        {
            // Se l'arma è già equipaggiata, potenziala
            existingWeapon.UpgradeTier();
            Debug.Log($"{weaponToEquip.name} è stata potenziata!");
        }
        else
        {
            // Altrimenti, equipaggia la nuova arma
            weaponsEquipped.Add(weaponToEquip);
            Debug.Log($"{weaponToEquip.name} è stata equipaggiata!");
        }
    }

    public void UnEquipWeapon(BaseWeapon weapon)
    {
        // Non si usa per ora ma non si sa mai
    }

    #endregion
}
