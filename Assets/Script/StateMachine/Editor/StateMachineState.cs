using System;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace StateMachineSpace.Editor
{
    public class StateMachineState : Node
    {
        private State _state;
        private StateMachineSchema _stateMachine;

        private Port _input;
        private Port _output;

        public State State { get { return _state; } private set { _state = value; } }
        public Port Input => _input;
        public Port Output => _output;

        public event Action StateSelected;

        public StateMachineState(State state, StateMachineSchema currentStateMachine)
        {
            _state = state;
            _stateMachine = currentStateMachine;

            title = state.GetType().Name;

            style.left = state.GraphPosition.x;
            style.top = state.GraphPosition.y;
        
            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            _input = InstantiatePort(Orientation.Horizontal, 
                                        Direction.Input, 
                                        Port.Capacity.Multi, 
                                        typeof(StateMachineState));
            _input.portName = "IN";
            inputContainer.Add(_input);
        }

        private void CreateOutputPorts()
        {
            _output = InstantiatePort(Orientation.Horizontal, 
                                        Direction.Output, 
                                        Port.Capacity.Multi, 
                                        typeof(StateMachineState));
            _output.portName = "OUT";
            outputContainer.Add(_output);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _state.GraphPosition = new float2(newPos.x, newPos.y);
        }

        public override void OnSelected()
        {
            StateSelected?.Invoke();
        }
    }
}
