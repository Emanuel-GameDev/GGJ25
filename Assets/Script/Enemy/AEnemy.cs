using UnityEngine;

public abstract class AEnemy : MonoBehaviour, IDamageable, IDamager
{
    [SerializeField] private int _health = 100;
    public int Health => _health;

    [SerializeField] private float _baseDamage = 10f;
    [field: SerializeField] public float damage { get; set; }
    [field: SerializeField] public float attackRate { get; set; }

    [SerializeField] private float _speed = 5f;
    public float Speed => _speed;

    [SerializeField] private GameObject _expDropPrefab;

    void Awake()
    {
        damage = _baseDamage;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //DROP ExP
        Destroy(gameObject);
    }
}
