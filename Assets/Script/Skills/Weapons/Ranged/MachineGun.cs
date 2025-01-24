using UnityEngine;

public class MachineGun : BaseWeapon
{
    [SerializeField]
    private float _projectileSpeed = 10f;

    [SerializeField]
    private int _projectileDmg = 2;

    [SerializeField]
    private int _fireAmount = 10;

    [SerializeField]
    private int _fireDuration = 4;

    [SerializeField]
    protected float _cooldown = 5f;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField, Tooltip("i proiettili da parte nella pool")]
    private int poolSize = 60;


    [Header("TIER 1")]

    [SerializeField, Range(1, 100)]
    private int _durationMultiplier = 15;

    [SerializeField]
    private int _fireAmountMultiplier = 2;


    [Header("TIER 2")]

    [SerializeField, Range(1, 100)]
    private int _cooldownAdd = 25;

    [SerializeField]
    private int _dmgAdd = 1;
}
