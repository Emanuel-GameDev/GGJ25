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
    public PlayerHandler playerHandler;

    [SerializeField]
    protected WeaponClass _weaponClass;

    [SerializeField]
    protected WeaponType _weaponType;

    protected int tierCounter = 0;

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
