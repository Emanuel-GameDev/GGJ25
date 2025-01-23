using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class ReachTargetState : State
{
    public Transform target;
    private AEnemy _enemyAgent;
    private Rigidbody2D _enemyAgentRb;

    public override UniTask OnEnter(GameObject agent)
    {
        if(agent.TryGetComponent(out AEnemy enemyAgent))
        {
            _enemyAgent = enemyAgent;
        }

        if(agent.TryGetComponent(out Rigidbody2D rigidbody))
        {
            _enemyAgentRb = rigidbody;
        }

        return base.OnEnter(agent);
    }

    public override UniTask OnUpdate(GameObject agent)
    {
        _enemyAgentRb.MovePosition(Vector2.MoveTowards(_enemyAgentRb.position, target.position, 0.1f));

        return base.OnUpdate(agent);
    }

    public override UniTask OnExit(GameObject agent)
    {
        return base.OnExit(agent);
    }

    public override void OnClone(ref State newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
    }
}
