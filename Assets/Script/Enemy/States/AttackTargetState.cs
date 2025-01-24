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
        var collider = agent.GetComponentsInChildren<Collider2D>()[1];
        if(collider != null)
        {
            // Debug.Log("Collider found: " + collider.gameObject.name);
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
        // Debug.Log("attack rate " + _damager.attackRate);
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
