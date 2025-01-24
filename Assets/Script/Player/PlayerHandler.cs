using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : Player
{
    #region Vars

    #region Weapons

    [SerializeField] List<BaseWeapon> weaponsEquipped;

    [SerializeField]
    protected float _fireRate = .3f;


    private bool canShoot = true;

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
            Debug.Log("shooting in update handler");
            if (canShoot)
            {
                Debug.Log("shooting in update handler pt2");
                ShootAll();
                canShoot = false;
                StartCoroutine(CooldownShooting());
            }
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

    public void EquipWeapon(BaseWeapon weaponToEquip)
    {
        weaponToEquip.playerHandler = this;
        weaponsEquipped.Add(weaponToEquip);
    }

    public void UnEquipWeapon(BaseWeapon weapon)
    {
        // Non si usa per ora ma non si sa mai
    }

    IEnumerator CooldownShooting()
    {
        yield return new WaitForSeconds(_fireRate);
        canShoot = true;
    }

    #endregion
}
