using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineSpace.Editor
{
    public class StateMachineTransition : Edge
    {
        private Transition _transitionData;
        public Transition transitionData { get { return _transitionData; } set { _transitionData = value; } }
        private StateMachineSchema _stateMachineSchema;

        public UnityAction TransitionSelected;

        public StateMachineTransition() { }

        public StateMachineTransition(Transition transition, StateMachineSchema stateMachine)
        {
            _transitionData = transition;
            _stateMachineSchema = stateMachine;
        }

        public override void OnSelected()
        {
            // Debug.Log("Transition Selected: " + _transitionData);
            TransitionSelected?.Invoke();
        }
    }

    public class StateMachineTransitionConnectorListener : IEdgeConnectorListener
    {
        private readonly Action<Edge> _onDrop;

        public StateMachineTransitionConnectorListener(Action<Edge> onDrop)
        {
            _onDrop = onDrop;
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            _onDrop?.Invoke(edge);
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position){}
    }
}