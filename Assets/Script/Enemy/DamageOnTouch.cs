using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    private IDamager _damager;
    
    void Awake()
    {
        _damager = GetComponentInParent<IDamager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage((int)_damager.damage);
        }
    }
}
