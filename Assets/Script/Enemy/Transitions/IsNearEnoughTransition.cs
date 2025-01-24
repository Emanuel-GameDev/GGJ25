using System;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class IsNearEnoughTransition : Transition
{
    //quanto è vicino alla bolla?
    public float distance;
    private GameObject target;

    public override bool ShouldTransition(StateMachine stateMachine, GameObject agent)
    {
        return Vector2.Distance(agent.transform.position, BubbleController.Instance.transform.position) < distance;
    }

    public override void OnClone(ref Transition newObject, GameObject agent)
    {
        base.OnClone(ref newObject, agent);
        ((IsNearEnoughTransition)newObject).distance = distance;
    }
}
