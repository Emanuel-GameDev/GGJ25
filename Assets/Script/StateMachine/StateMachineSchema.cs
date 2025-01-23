using UnityEngine;

namespace StateMachineSpace
{
    [CreateAssetMenu(menuName = "State Machine", fileName = "NewStateMachine")]
    public class StateMachineSchema : ScriptableObject
    {
        public StateMachine StateMachine;
    }
}