using System;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class IsNearEnoughTransition : Transition
{
    public float distance;

    public override bool ShouldTransition(StateMachine stateMachine, GameObject agent)
    {
        if(BubbleController.Instance == null) return false;
        
        return Vector2.Distance(agent.transform.position, BubbleController.Instance.transform.position) < distance;
    }

    public override void OnClone(ref Transition newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
        ((IsNearEnoughTransition)newObject).distance = distance;
    }
}
