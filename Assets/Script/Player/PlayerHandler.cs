using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : Player
{
    #region Vars

    #region Weapons

    [SerializeField] List<BaseWeapon> weaponsEquipped;

    [SerializeField]
    private GameObject weaponContainerObj;

    private PlayerController controller;

    #endregion

    #region Passives
    // Lista di passive
    #endregion

    #endregion

    #region UnityFunctions

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        GameHUDmanager.instance.LoadWeaponSprites(this, GetComponent<PlayerInput>().playerIndex);
    }

    private void Update()
    {
        if (controller.isShooting)
        {
            ShootAll();
        }
    }


    #endregion

    #region Weapons

    private void ShootAll()
    {
        for (int i = 0; i < weaponsEquipped.Count; i++)
        {
            weaponsEquipped[i].Shoot();
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

        BaseWeapon weapon = weaponToEquipObj.GetComponent<BaseWeapon>();

        if (weapon == null)
        {
            weapon = weaponToEquipObj.GetComponentInChildren<BaseWeapon>();
        }

        if (weapon == null)
            Debug.Log("è null porcoddio");

        if (weaponsEquipped.Count < 3)
        {
            weaponsEquipped.Add(weapon);
            weapon.playerHandler = this;
        }
        else
            Debug.Log("NUMERO MASSIMO DI ARMI EQUIPAGGIATE");

        GameHUDmanager.instance.LoadWeaponSprites(this, GetComponent<PlayerInput>().playerIndex);
    }

    public void UnEquipWeapon(BaseWeapon weapon)
    {
        // Non si usa per ora ma non si sa mai
    }

    #endregion
}
