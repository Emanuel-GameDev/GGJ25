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
    public string description;
    public string tier1Description;
    public string tier2Description;
    public Sprite sprite;
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
        {
            tierCounter++;

            if (tierCounter == 3)
            {
                GameObject w = WeaponManager.Instance.database.weaponDatabase.Find(w => w.name == name);
                WeaponManager.Instance.database.weaponDatabase.Remove(w);
            }
        }
    }
}
