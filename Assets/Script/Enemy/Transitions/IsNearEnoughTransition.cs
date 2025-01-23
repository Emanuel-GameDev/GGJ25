using System;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class IsNearEnoughTransition : Transition
{
    //quanto è vicino alla bolla?
    public float distance;

    public override bool ShouldTransition(StateMachine stateMachine, GameObject agent)
    {
        throw new System.NotImplementedException();
    }

    public override void OnClone(ref Transition newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
        ((IsNearEnoughTransition)newObject).distance = distance;
    }
}
