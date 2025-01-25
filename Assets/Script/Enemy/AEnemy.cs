using Boids;
using StateMachineSpace;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour, IDamageable, IDamager, IPauseable
{
    [SerializeField] private float _health = 100;
    public float Health => _health;

    [SerializeField] private float _baseDamage = 10f;
    [field: SerializeField] public float damage { get; set; }
    [field: SerializeField] public float attackRate { get; set; }

    [SerializeField] protected float _speed = 5f;
    public float Speed => _speed;

    [SerializeField] private GameObject[] _expDropPrefab;

    protected virtual void Awake()
    {
        damage = _baseDamage;
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        Debug.Log("Damage taken: " + damage);
        if (_health <= 0)
        {
            Die();
            return;
        }
    }

    protected virtual void Die()
    {
        var firstPlayerLevelManager = ControllerPlayersManager.Instance.Players[0].gameObject.GetComponentInChildren<PlayerLevelManager>();
        
        PlayerLevelManager secondPlayerLevelManager = null;
        if(ControllerPlayersManager.Instance.Players.Count > 1)
            secondPlayerLevelManager = ControllerPlayersManager.Instance.Players[1].gameObject.GetComponentInChildren<PlayerLevelManager>();
        
        if(firstPlayerLevelManager.Level <= 10
            || (secondPlayerLevelManager != null && secondPlayerLevelManager.Level <= 10))
        {
            var expDrop = Instantiate(_expDropPrefab[0], transform.position, Quaternion.identity);
            var expValue = expDrop.GetComponent<ExpItem>().EXPValue;
            expDrop.GetComponent<ExpItem>().EXPValue += expValue * (firstPlayerLevelManager.Level / 10f);
        }
        else if(firstPlayerLevelManager.Level <= 20
            || (secondPlayerLevelManager != null && secondPlayerLevelManager.Level <= 20))
        {
            var expDrop = Instantiate(_expDropPrefab[1], transform.position, Quaternion.identity);
            var expValue = expDrop.GetComponent<ExpItem>().EXPValue;
            expDrop.GetComponent<ExpItem>().EXPValue += expValue * (firstPlayerLevelManager.Level / 10f);
        }
        else if(firstPlayerLevelManager.Level > 20
            || (secondPlayerLevelManager != null && secondPlayerLevelManager.Level > 20))
        {
            var expDrop = Instantiate(_expDropPrefab[2], transform.position, Quaternion.identity);
            var expValue = expDrop.GetComponent<ExpItem>().EXPValue;
            expDrop.GetComponent<ExpItem>().EXPValue += expValue * (firstPlayerLevelManager.Level / 10f);
        }

        EventManager.OnEnemyDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public void Pause()
    {
        GetComponent<StateMachineRunner>().PauseStateMachine();
        GetComponent<Agent>().PauseAgent();
    }

    public void Unpause()
    {
        GetComponent<StateMachineRunner>().StartStateMachine();
        GetComponent<Agent>().UnpauseAgent();
    }

    public void TakeOxygen(float damage)
    {
        
    }
}
