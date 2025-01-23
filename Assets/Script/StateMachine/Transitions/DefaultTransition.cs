using System;
using StateMachineSpace;
using UnityEngine;

[Serializable]
public class DefaultTransition : Transition
{
    public float value;
    public override bool ShouldTransition(StateMachineSpace.StateMachine stateMachine, GameObject target)
    {
        return false;
    }
}