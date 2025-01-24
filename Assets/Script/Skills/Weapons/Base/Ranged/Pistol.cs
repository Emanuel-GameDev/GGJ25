using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pistol : BaseWeapon
{
    [SerializeField]
    private float _projectileSpeed = 10f;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float cooldown = 1.5f;

    [SerializeField, Tooltip("i proiettili da parte nella pool")]
    private int poolSize = 80;


    private GameObject pistolProjectilePool;
    private List<GameObject> projectilePool;
    private int currentPoolIndex = 0;

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
            GameObject projectile = Instantiate(projectilePrefab, pistolProjectilePool.transform);
            projectile.SetActive(false);
            projectilePool.Add(projectile);
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        GameObject projectile = GetPooledProjectile();
        if (projectile != null)
        {
            projectile.transform.position = transform.position;
            // dir mirino
            if (playerHandler != null && projectile != null)
            {
                //Vector3 direction = playerHandler.sight.transform.position - projectile.transform.position; // Direzione verso il mirino
                //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Angolo in gradi
                var sightRot = playerHandler.sight.transform.localRotation;
                projectile.transform.localRotation = sightRot; // Ruota solo la Z
            }

            projectile.SetActive(true);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("SHOOT " + projectile.transform.forward);
                var sightObjectRef = playerHandler.sight.gameObject.transform.GetChild(0).position;
                rb.AddForce((sightObjectRef - projectile.transform.position) * _projectileSpeed, ForceMode2D.Impulse);
            }

            StartCoroutine(CooldownProjectile(projectile));
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

    IEnumerator CooldownProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(cooldown);
        projectile.SetActive(false);
    }
}
