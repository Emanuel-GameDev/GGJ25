using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BaseWeapon
{
    [SerializeField]
    private float _projectileSpeed = 10f;

    [SerializeField]
    private int _projectileDmg = 2;

    [SerializeField]
    private int _fireAmountPerSeconds = 10;

    [SerializeField]
    private float _fireDuration = 4;

    [SerializeField]
    protected float _cooldown = 5f;

    [SerializeField]
    private float angleSpread = 60;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField, Tooltip("i proiettili da parte nella pool")]
    private int poolSize = 60;


    [Header("TIER 1")]

    [SerializeField, Range(1, 100)]
    private float _durationMultiplier = 15;

    [SerializeField]
    private int _fireAmountMultiplier = 2;


    [Header("TIER 2")]

    [SerializeField, Range(-100, 0)]
    private int _cooldownMultiplier = 25;

    [SerializeField]
    private int _dmgAdd = 1;


    private bool canShoot = true;
    private int currentPoolIndex = 0;
    private GameObject pistolProjectilePool;
    private List<GameObject> projectilePool;

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

    public override void Shoot()
    {
        Debug.Log("Start Shoot");
        base.Shoot();

        if (!canShoot) return;
        Debug.Log("Shooting");
        canShoot = false;
        StartCoroutine(ShootingRepeat());

    }

    IEnumerator ShootingRepeat()
    {
        float elapsedTime = 0f;
        float shotInterval = 1f / _fireAmountPerSeconds;

        while (elapsedTime < _fireDuration)
        {

            GameObject projectile = GetPooledProjectile();
            Fire(projectile);

            yield return new WaitForSeconds(shotInterval);

            elapsedTime += shotInterval;

        }

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        canShoot = true;
    }

    private void Fire(GameObject projectile)
    {
        if (projectile != null)
        {
            projectile.GetComponent<RayPistolProjectile>()._baseDmg = _projectileDmg;
            projectile.transform.position = transform.position;

            float randPos = Random.Range(-(angleSpread / 2), (angleSpread / 2));
            Debug.Log(randPos);

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
                var newPos = new Vector3(sightObjectRef.x + randPos, sightObjectRef.y + randPos);
                rb.AddForce((newPos - projectile.transform.position) * _projectileSpeed, ForceMode2D.Impulse);
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

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {
            float percentage = (float)((_durationMultiplier / 100f)+1f);
            _fireDuration *= percentage;

            _fireAmountPerSeconds += _fireAmountMultiplier;
        }

        if (tierCounter == 2)
        {
            float percentage = (float)_cooldownMultiplier / 100f;
            _cooldown *= percentage;

            _projectileDmg += _dmgAdd;
        }
    }
}
