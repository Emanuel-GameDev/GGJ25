using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _oxygen = 100f;
    [SerializeField] private float _maxOxygen = 100f;
    [SerializeField] private bool _carryBubble = false;
    [SerializeField] private float _oxygenLossRate = 1f;
    [SerializeField] private float _healthLossRate = 1f;

    [SerializeField] private float _invincibilityTime = 2f;
    [SerializeField] private bool _invincible = false;

    void Update()
    {
        if(!_carryBubble
            && _oxygen > 0)
        {
            _oxygen -= _oxygenLossRate * Time.deltaTime;
        }

        if(_oxygen <= 0)
        {
            _health -= _healthLossRate * Time.deltaTime;
        }
    }

    public void SetCarryBubble(bool isCarringBubble)
    {
        _carryBubble = isCarringBubble;
    }

    public void TakeDamage(float damage)
    {
        if(_invincible)
            return;

        _health -= damage;
        
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            _invincible = true;
            InvincibilityTimer().Forget();
        }
    }

    private async UniTask InvincibilityTimer()
    {
        await UniTask.WaitForSeconds(_invincibilityTime);
        _invincible = false;
    }

    private void Die()
    {
        EventManager.OnPlayerDeath?.Invoke();
    }
}
