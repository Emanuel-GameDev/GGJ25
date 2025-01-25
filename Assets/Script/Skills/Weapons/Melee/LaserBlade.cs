using System.Collections;
using UnityEngine;

public class LaserBlade : BaseWeapon
{
    [SerializeField]
    private float _baseDmg;

    [SerializeField]
    private float _hitRate;

    [Header("Tier 1")]

    [SerializeField, Range(1, 100)]
    private int hitAreaModifierTier1 = 50;

    [Header("Tier2")]

    [SerializeField]
    private int _dmgAddiction = 5;

    [SerializeField, Range(1, 100)]
    private int hitAreaModifierTier2 = 20;

    [SerializeField, Range(1, 100)]
    private int distanceModifier = 20;


    private Animator anim;
    private bool canAttack = true;
    private GameObject bladePivot;
    private Transform sightRef;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bladePivot = transform.parent.gameObject;

        sightRef = playerHandler.sight.transform;
    }   

    private void Update()
    {
        //Distanza mirino player
        Vector2 distance = sightRef.GetChild(0).position - bladePivot.transform.position;
        distance.Normalize();

        if (distance == Vector2.left)
            bladePivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        if (distance == Vector2.right)
        {
            Debug.Log("DX");
            bladePivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (distance == Vector2.up)
        {
            Debug.Log("SU");
            bladePivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        if (distance == Vector2.down)
        {
            Debug.Log("GIU");
            bladePivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {
            int percentageModifier = hitAreaModifierTier1 / 100;
            bladePivot.transform.localScale = new Vector2(bladePivot.transform.localScale.x * percentageModifier, bladePivot.transform.localScale.y * percentageModifier);
        }
        if (tierCounter == 2)
        {
            _baseDmg += _dmgAddiction;

            int percentageModifier = hitAreaModifierTier2 / 100;
            bladePivot.transform.localScale = new Vector2(bladePivot.transform.localScale.x * percentageModifier, bladePivot.transform.localScale.y * percentageModifier);

            transform.localPosition = new Vector2(transform.localPosition.x * (distanceModifier * 100), transform.localPosition.y);
        }
    }

    public override void Shoot()
    {
        base.Shoot();

        if (!canAttack) return;

        anim.SetTrigger("Attack");
        canAttack = false;
        StartCoroutine(CooldownAttacking());
    }

    IEnumerator CooldownAttacking()
    {
        yield return new WaitForSeconds(_hitRate);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_baseDmg);
        }
    }
}
