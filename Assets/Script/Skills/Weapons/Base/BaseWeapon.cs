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
    protected float _baseDmg = 5f;

    [SerializeField]
    protected WeaponClass _weaponClass;

    [SerializeField]
    protected WeaponType _weaponType;

    #endregion


    public virtual void Shoot()
    {
        //Debug.Log("Sparo un colpo dal BaseWeapon");
    }
}
