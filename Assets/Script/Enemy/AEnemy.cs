using UnityEngine;

public abstract class AEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 100;
    public int Health => _health;

    public void EnemyAttackBehavior() 
    {

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
        Destroy(gameObject);
    }
}
