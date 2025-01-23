using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 100;
    [SerializeField] private float _oxygen = 100f;
    [SerializeField] private bool _carryBubble = false;
    [SerializeField] private float _oxygenLossRate = 1f;

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
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        EventManager.OnPlayerDeath?.Invoke();
    }
}
