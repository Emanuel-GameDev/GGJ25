using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _oxygen = 100f;
    public float Oxygen => _oxygen;
    [SerializeField] private float _maxOxygen = 100f;
    [SerializeField] private bool _carryBubble = false;
    public bool CarryBubble => _carryBubble;
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

        BarsUI.instance.SetMaxHealth(_maxHealth, GetComponent<PlayerInput>().playerIndex);
        BarsUI.instance.SetMaxOxygen(_maxOxygen, GetComponent<PlayerInput>().playerIndex);
        BarsUI.instance.SetExp((float)0, GetComponent<PlayerInput>().playerIndex);
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
            BarsUI.instance.SetHealth(_health, GetComponent<PlayerInput>().playerIndex);
        }

        BarsUI.instance.SetOxygen(_oxygen, GetComponent<PlayerInput>().playerIndex);
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

        BarsUI.instance.SetHealth(_health, GetComponent<PlayerInput>().playerIndex);
        
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
        var pauseManager = FindAnyObjectByType<PauseManager>();

        if (pauseManager != null)
        {
            pauseManager.pauseAction.Disable();
            pauseManager.PauseAll();
        }

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

    public void TakeOxygen(float damage)
    {
        _oxygen -= damage;
        if(_oxygen < 0)
        {
            _oxygen = 0;
        }
    }

    public void RegenerateHealth(float amount)
    {
        _health += amount;
        if(_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void RiseOxygenGainRate(int amount)
    {
        _oxygenGainRate += _oxygenGainRate * amount / 100f;
    }
}
