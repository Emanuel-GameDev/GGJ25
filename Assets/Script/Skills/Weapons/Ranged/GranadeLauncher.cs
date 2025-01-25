using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeLauncher : BaseWeapon
{
    [SerializeField]
    private float _projectileSpeed = 10f;

    [SerializeField]
    private int _projectileDmg = 30;

    [SerializeField]
    private float _fireRate = 5f;


    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField, Tooltip("i proiettili da parte nella pool")]
    private int poolSize = 60;


    [Header("TIER 1")]

    [SerializeField, Range(1, 100)]
    private float _scaleIncrease = 20f;

    [SerializeField, Range(1, 100)]
    private float _fireRateModifier = 20;


    [Header("TIER 2")]

    [SerializeField]
    private int _dmgModifier = 10;



    private GameObject pistolProjectilePool;
    private List<GameObject> projectilePool;
    private int currentPoolIndex = 0;
    private bool canShoot = true;


    private void Awake()
    {
        InitializePool();
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

    public override void Shoot()
    {
        base.Shoot();

        if (!canShoot) return;

        GameObject projectile = GetPooledProjectile();
        Fire(projectile);
    }

    private void Fire(GameObject projectile)
    {
        if (projectile != null)
        {
            canShoot = false;

            projectile.GetComponent<Granade>()._dmg = _projectileDmg;
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
                var sightObjectRef = playerHandler.sight.gameObject.transform.GetChild(0).position;
                rb.AddForce((sightObjectRef - projectile.transform.position) * _projectileSpeed, ForceMode2D.Impulse);
            }

            StartCoroutine(Cooldown());
        }

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_fireRate);
        canShoot = true;
    }

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {
            float scaleIncreasePercentage = _scaleIncrease / 100;
            projectilePrefab.gameObject.transform.localScale = new Vector3(projectilePrefab.gameObject.transform.localScale.x + scaleIncreasePercentage,
                                                                            projectilePrefab.gameObject.transform.localScale.y + scaleIncreasePercentage);

            float percentage = _fireRateModifier / 100;
            _fireRate *= percentage;
        }

        if (tierCounter == 2)
        {
            _projectileDmg += _dmgModifier;
        }
    }
}
