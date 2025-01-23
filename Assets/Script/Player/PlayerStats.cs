using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 100;
    [SerializeField] private float _oxygen = 100f;
    [SerializeField] private bool _carryBubble = false;
    [SerializeField] private float _oxygenLossRate = 1f;

    [SerializeField] private float _invincibilityTime = 2f;
    [SerializeField] private bool _invincible = false;

    void Update()
    {
        if(!_carryBubble)
        {
            _oxygen -= _oxygenLossRate * Time.deltaTime;
        }
    }

    public void SetCarryBubble(bool isCarringBubble)
    {
        _carryBubble = isCarringBubble;
    }

    public void TakeDamage(int damage)
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
