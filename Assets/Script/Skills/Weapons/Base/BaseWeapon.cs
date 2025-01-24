using UnityEngine;

public enum WeaponClass
{
    Ranged,
    Melee
}

public enum WeaponType
{
    projectile,
    continuous,
    wave,
    spin,
    target
}

public abstract class BaseWeapon : MonoBehaviour
{
    #region vars

    public string weaponName;
    public PlayerHandler playerHandler;

    public WeaponClass _weaponClass;

    public WeaponType _weaponType;

    public int tierCounter = 0;

    #endregion


    public virtual void Shoot()
    {
        //Debug.Log("Sparo un colpo dal BaseWeapon");
    }

    public virtual void UpgradeTier()
    {
        if (tierCounter < 3)
            tierCounter++;
    }
}
