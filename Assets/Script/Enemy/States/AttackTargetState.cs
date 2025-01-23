using System;
using Cysharp.Threading.Tasks;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class AttackTargetState : State
{
    public override UniTask OnEnter(GameObject agent)
    {
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
