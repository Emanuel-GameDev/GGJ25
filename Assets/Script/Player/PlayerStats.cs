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
    [SerializeField] private float _oxygenGainRate = 2f;
    [SerializeField] private float _healthLossRate = 1f;

    [SerializeField] private float _invincibilityTime = 2f;
    [SerializeField] private bool _invincible = false;

    private bool _isInPause = false;

    void Awake()
    {
        EventManager.OnBubbleGrabbed += SetBubbleCarring;
        EventManager.OnBubbleThrown += SetBubbleCarring;
    }

    void Update()
    {
        if(_isInPause)
            return;

        if(!_carryBubble
            && _oxygen > 0)
        {
            _oxygen -= _oxygenLossRate * Time.deltaTime;
        }
        else if(_carryBubble 
                && _oxygen < _maxOxygen)
        {
            _oxygen += _oxygenGainRate * Time.deltaTime;
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
        await UniTask.WaitForSeconds(_invincibilityTime, true);
        _invincible = false;
    }

    private void Die()
    {
        EventManager.OnPlayerDeath?.Invoke();
    }

    private void SetBubbleCarring(GameObject player)
    {
        // Debug.Log("Player: " + player.name);
        if(player == gameObject)
        {
            // Debug.Log("TRUE Player: " + player.name);
            if(_carryBubble)
                _carryBubble = false;
            else
                _carryBubble = true;
        }
    }

    public void Pause()
    {
        _isInPause = true;
    }

    public void Unpause()
    {
        _isInPause = false;
    }
}
