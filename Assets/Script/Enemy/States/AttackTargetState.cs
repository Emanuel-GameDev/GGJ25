using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using Unity.Burst;
using UnityEngine;

[Serializable]
public class AttackTargetState : State
{
    private Collider2D _collider;
    private IDamager _damager;

    [BurstCompile]   
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

    [BurstCompile]
    public override async UniTask OnUpdate(GameObject agent)
    {
        if (agent == null) return;

        // Verifica se il collider esiste prima di accedere ad esso
        if (_collider == null)
        {
            Debug.LogWarning("Collider is missing or has been destroyed!");
            return;
        }

        _collider.enabled = true;

        await UniTask.Delay((int)(_damager.attackRate * 1000));

        // Verifica di nuovo se il collider esiste prima di accedervi
        if (_collider == null)
        {
            Debug.LogWarning("Collider was destroyed during the delay!");
            return;
        }

        _collider.enabled = false;

        await base.OnUpdate(agent);
    }

    [BurstCompile]
    public override UniTask OnExit(GameObject agent)
    {
        // Debug.Log("AttackTargetState exit");
        _collider.enabled = false;
        return base.OnExit(agent);
    }

    public override void OnClone(ref State newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
    }
}
