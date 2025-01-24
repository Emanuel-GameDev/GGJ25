using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class ReachTargetState : State
{
    public AEnemy _enemyAgent;
    public Rigidbody2D _enemyAgentRb;

    public override UniTask OnEnter(GameObject agent)
    {

        return base.OnEnter(agent);
    }

    public override UniTask OnUpdate(GameObject agent)
    {
        //TODO animazione e flip enemy
        return base.OnUpdate(agent);
    }

    public override UniTask OnExit(GameObject agent)
    {
        return base.OnExit(agent);
    }

    public override void OnClone(ref State newObject, GameObject agent)
    {
        if(agent.TryGetComponent(out AEnemy enemyAgent))
        {
            _enemyAgent = enemyAgent;
        }

        if(agent.TryGetComponent(out Rigidbody2D rigidbody))
        {
            _enemyAgentRb = rigidbody;
        }

        base.OnClone(ref newObject, agent);
        ((ReachTargetState)newObject)._enemyAgentRb = _enemyAgentRb;
        ((ReachTargetState)newObject)._enemyAgent = _enemyAgent;
    }
}
