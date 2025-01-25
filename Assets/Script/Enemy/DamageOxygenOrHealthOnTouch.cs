using UnityEngine;

public class DamageOxygenOrHealthOnTouch : MonoBehaviour
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
            if(other.gameObject.TryGetComponent(out PlayerStats playerStats))
            {
                if(playerStats.Oxygen > 0)
                {
                    playerStats.TakeOxygen(_damager.damage);
                }
                else
                {
                    damageable.TakeDamage((int)_damager.damage);
                }
            }
        }
    }
}
