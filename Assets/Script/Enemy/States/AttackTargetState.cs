using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class AttackTargetState : State
{
    Collider2D _collider;
    
    public override UniTask OnEnter(GameObject agent)
    {
        if(agent.GetComponentInChildren<Collider2D>())
        {
            _collider.enabled = true;
        }

        return base.OnEnter(agent);
    }

    public override UniTask OnUpdate(GameObject agent)
    {
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
