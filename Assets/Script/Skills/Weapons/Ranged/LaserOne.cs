using System.Collections;
using UnityEngine;

public class LaserOne : BaseWeapon
{
    [SerializeField]
    private float _rayLenght = 5f;

    [SerializeField]
    private float _raySpeed = 10f;

    [SerializeField]
    private int _baseDmg = 10;

    [SerializeField]
    private int _baseMaxEnemyHit = 4;


    [Header("Tier 1")]

    [SerializeField]
    private int _modifierEnemyHitTier1 = 1;

    [Header("Tier 1")]

    [SerializeField]
    private int _modifierEnemyHitTier2 = 2;

    [SerializeField, Range(-100, 100)]
    private int _modifierDmg = 5;


    private bool canShoot = true;
    private int enemyHit = 0;
    private Vector2 originalScale;

    private void Start()
    {
        gameObject.SetActive(false);
        originalScale = transform.localScale;
    }

    public override void Shoot()
    {
        base.Shoot();

        if (!canShoot) return;

        Vector2 distance = playerHandler.sight.transform.GetChild(0).position - transform.position;
        distance.Normalize();

        if (distance == Vector2.left)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        if (distance == Vector2.right)
        {
            Debug.Log("DX");
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (distance == Vector2.up)
        {
            Debug.Log("SU");
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        if (distance == Vector2.down)
        {
            Debug.Log("GIU");
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }

        canShoot = false;
        enemyHit = 0;
        gameObject.SetActive(true);
        StartCoroutine(ScaleSpriteX());
    }

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {
            _baseMaxEnemyHit += _modifierEnemyHitTier1;
        }
        if (tierCounter == 2)
        {
            _baseMaxEnemyHit += _modifierEnemyHitTier2;
            _baseDmg += _modifierDmg;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyHit >= _baseMaxEnemyHit)
        {
            StopCoroutine(ScaleSpriteX());
            transform.localScale = originalScale;
            enemyHit = 0;
            canShoot = true;
            return;
        }

        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_baseDmg);
            enemyHit++;
        }
    }

    private IEnumerator ScaleSpriteX()
    {
        while (transform.localScale.x < _rayLenght)
        {
            float scaleIncrement = _raySpeed * Time.deltaTime;
            transform.localScale += new Vector3(scaleIncrement, 0f, 0f);

            yield return null;
        }

        transform.localScale = originalScale;
        gameObject.SetActive(false);
        canShoot = true;
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true ;
    }
}
