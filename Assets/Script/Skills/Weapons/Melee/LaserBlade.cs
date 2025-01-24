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
    private int hitAreaModifier = 50;

    [Header("Tier2")]

    [SerializeField]
    private int _dmgAddiction = 5;


    private Animator anim;
    private bool canAttack = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void UpgradeTier()
    {
        base.UpgradeTier();

        if (tierCounter == 1)
        {

        }
    }

    public override void Shoot()
    {
        base.Shoot();

        if (!canAttack) return;

        anim.SetTrigger("Attack");
    }

    IEnumerator CooldownAttacking()
    {
        yield return new WaitForSeconds(_hitRate);
        canAttack = true;
    }
}
