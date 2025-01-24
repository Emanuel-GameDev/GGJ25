using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pistol : BaseWeapon
{
    [SerializeField]
    private float _projectileSpeed = 10f;

    [SerializeField]
    private int _projectileDmg = 5;

    [SerializeField]
    protected float _fireRate = 1f;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField, Tooltip("i proiettili da parte nella pool")]
    private int poolSize = 60;

    [Header("TIER 1")]

    public bool tier1Unlocked = false;

    [SerializeField]
    private int tier1UpgradeDmg = 5;

    [SerializeField, Range(0, 100)]
    private int tier1UpgradeFireRate = 50;

    [Header("TIER 2")]

    public bool tier2Unlocked = false;


    private GameObject pistolProjectilePool;
    private List<GameObject> projectilePool;
    private int currentPoolIndex = 0;
    private bool canShoot = true;

    private void Awake()
    {
        InitializePool();
    }

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {
            _projectileDmg += tier1UpgradeDmg;
            _fireRate *= (tier1UpgradeFireRate / 100);

            tier1Unlocked = true;
        }
    }

    private void InitializePool()
    {
        pistolProjectilePool = new GameObject();

        projectilePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            InstantiateProjectile();
        }
    }

    private void InstantiateProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, pistolProjectilePool.transform);
        projectile.SetActive(false);
        projectilePool.Add(projectile);
    }

    public override void Shoot()
    {
        base.Shoot();

        if (!canShoot) return;

        GameObject projectile = GetPooledProjectile();
        Fire(projectile);

        if (tierCounter == 2)
        {
            if (tier2Unlocked == false)
                tier2Unlocked = true;

            GameObject projectile2 = GetPooledProjectile();
            Fire(projectile2);
        }

    }

    private void Fire(GameObject projectile)
    {
        if (projectile != null)
        {
            projectile.GetComponent<RayPistolProjectile>()._baseDmg = _projectileDmg;
            projectile.transform.position = transform.position;

            if (playerHandler != null && projectile != null)
            {
                var sightRot = playerHandler.sight.transform.localRotation;
                projectile.transform.localRotation = sightRot;
            }

            projectile.SetActive(true);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                //Debug.Log("SHOOT " + projectile.transform.forward);
                var sightObjectRef = playerHandler.sight.gameObject.transform.GetChild(0).position;
                rb.AddForce((sightObjectRef - projectile.transform.position) * _projectileSpeed, ForceMode2D.Impulse);

                canShoot = false;
                StartCoroutine(CooldownShooting());
            }
        }

    }

    private GameObject GetPooledProjectile()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int index = (currentPoolIndex + i) % poolSize;
            if (!projectilePool[index].activeInHierarchy)
            {
                currentPoolIndex = index;
                return projectilePool[index];
            }
        }

        // Nessun proiettile disponibile
        Debug.LogWarning("Tutti i proiettili nella pool sono attivi! aumenta la poolSize");
        return null;
    }

    IEnumerator CooldownShooting()
    {
        yield return new WaitForSeconds(_fireRate);
        canShoot = true;
    }
}
