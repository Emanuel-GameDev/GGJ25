using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class AttackTargetState : State
{
    private Collider2D _collider;
    private IDamager _damager;
    
    public override UniTask OnEnter(GameObject agent)
    {
        var collider = agent.GetComponentInChildren<Collider2D>();
        if(collider != null)
        {
            _collider = collider;
        }
        else
        {
            Debug.LogError("No collider found: " + agent.name);
        }

        if(agent.TryGetComponent(out IDamager damager))
        {
            _damager = damager;
        }

        return base.OnEnter(agent);
    }

    public override async UniTask OnUpdate(GameObject agent)
    {
        _collider.enabled = true;
        await UniTask.Delay((int)(_damager.attackRate * 1000));
        _collider.enabled = false;
        await base.OnUpdate(agent);
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
