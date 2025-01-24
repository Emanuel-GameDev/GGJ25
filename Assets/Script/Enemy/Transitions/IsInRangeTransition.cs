using System;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class IsInRangeTransition : Transition
{
    public float distance;

    public override bool ShouldTransition(StateMachine stateMachine, GameObject agent)
    {
        return Vector2.Distance(agent.transform.position, BubbleController.Instance.transform.position) > distance;
    }

    public override void OnClone(ref Transition newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
        ((IsInRangeTransition)newObject).distance = distance;
    }
}
