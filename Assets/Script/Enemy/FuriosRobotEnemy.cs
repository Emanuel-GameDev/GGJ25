using Boids;
using UnityEngine;

public class FuriosRobotEnemy : AEnemy
{
    [SerializeField] private float _speedBoost;
    [SerializeField] private bool _isFurious = false;

    Agent _agent;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<Agent>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (!_isFurious)
        {
            _agent.MaxLinearSpeed = _agent.MaxLinearSpeed + _agent.MaxLinearSpeed * _speedBoost / 100f;
            _isFurious = true;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
