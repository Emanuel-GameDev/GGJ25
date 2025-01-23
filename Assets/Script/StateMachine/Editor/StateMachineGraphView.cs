using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateMachineSpace.Editor
{
    public class StateMachineGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<StateMachineGraphView, UxmlTraits> { }
        private StateMachineSchema _currentStateMachine;
        public event Action<StateMachineState> NodeSelected; 
        public event Action<StateMachineTransition> EdgeSelected; 
        // public event Action<StateMachineState> ElementRemoved;//TODO: remove

        private bool isNewSchema = true;
        private StateMachineTransitionConnectorListener edgeConnectorListener;
        

        public StateMachineGraphView()
        {
            style.flexGrow = 1;
            style.height = new StyleLength(Length.Percent(100));
            style.width = new StyleLength(Length.Percent(100));

            var background = new GridBackground();
            background.EnableInClassList("grid", true);
            Insert(0, background);
            
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // edgeConnectorListener = new StateMachineTransitionConnectorListener(OnEdgeDrop);
        }

        public void PopulateView(StateMachineSchema currentStateMachine)
        {
            // Debug.Log("Populated view");

            if (_currentStateMachine == currentStateMachine)
            {
                return;
            }

            isNewSchema = true;

            DeleteElements(graphElements);
            graphViewChanged -= OnGraphViewChanged;
            _currentStateMachine = currentStateMachine;

            var nodeToView = new Dictionary<State, StateMachineState>();
            foreach (var state in _currentStateMachine.StateMachine.States)
            {
                nodeToView.Add(state, CreateStateNodeView(state));
            }

            foreach (var transition in _currentStateMachine.StateMachine.Transitions)
            {
                if(_currentStateMachine.StateMachine.States.Count == 0)
                    break;

                if(    transition.Start <= _currentStateMachine.StateMachine.States.Count - 1
                    && transition.End <= _currentStateMachine.StateMachine.States.Count - 1
                    && transition.Start != transition.End)
                {
                    var startState = _currentStateMachine.StateMachine.States[transition.Start];
                    var endState = _currentStateMachine.StateMachine.States[transition.End];
                    var edge = nodeToView[startState].Output.ConnectTo<StateMachineTransition>(nodeToView[endState].Input); 

                    edge.transitionData = transition;
                    edge.TransitionSelected += () =>
                    {
                        EdgeSelected?.Invoke(edge);
                    };

                    // CreateTransitionNodeView(transition)
                    // Debug.Log($"Populated: {edge.GetType()}");

                    AddElement(edge);
                }
            }

            graphViewChanged += OnGraphViewChanged;
        }


        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if(isNewSchema)
            {
                isNewSchema = false;
                return graphViewChange;
            }

            if(graphViewChange.elementsToRemove != null)
            {
                foreach(var elementToRemove in graphViewChange.elementsToRemove)
                {
                    switch(elementToRemove)
                    {
                        case StateMachineState stateMachineState:

                            // Debug.Log("State removed: " + stateMachineState.GetType());

                            // ElementRemoved?.Invoke(stateMachineState);

                            _currentStateMachine.StateMachine.States.Remove(stateMachineState.State);

                            RemoveElement(stateMachineState);

                            break;

                        case StateMachineTransition stateMachineTransition:

                            // Debug.Log("Transition removed: " + stateMachineTransition.GetType());

                            // ElementRemoved?.Invoke();
                            
                            _currentStateMachine.StateMachine.Transitions.Remove(stateMachineTransition.transitionData);
                            
                            RemoveElement(stateMachineTransition);

                            break;
                        case Edge edge:
                        
                            // Debug.Log("Edge removed: " + edge.GetType());

                            RemoveElement(edge);

                            break;
                        
                        default:
                            // Debug.Log("Unhandled element: " + elementToRemove + " " + elementToRemove.GetType());
                            break;
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                // Debug.Log("Edge count: " + graphViewChange.edgesToCreate.Count);

                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    if (edge.output.node is not StateMachineState startNode)
                    {
                        // Debug.Log("Invalid start node: " + edge.output.node);
                        continue;
                    }
                    
                    if (edge.input.node is not StateMachineState endNode)
                    {
                        Debug.LogError("Invalid end node: " + edge.input.node);
                        continue;
                    }

                    // Debug.Log("Valid Edge: " + edge.GetType().FullName);

                    // startNode.Output.ConnectTo<StateMachineTransition>(endNode.Input);
                }
            }

            return graphViewChange;
        }

        public void CreateElementByType(Type type)
        {
            switch(type.BaseType.Name)
            {
                case nameof(State):

                    var nodeState = Activator.CreateInstance(type) as State;
            
                    _currentStateMachine.StateMachine.States.Add(nodeState);
                    
                    CreateStateNodeView(nodeState);

                    EditorUtility.SetDirty(_currentStateMachine);
                    
                    break;

                case nameof(Transition):

                    var nodeTransition = Activator.CreateInstance(type) as Transition;
            
                    _currentStateMachine.StateMachine.Transitions.Add(nodeTransition);
                    
                    CreateTransitionEdgeView(nodeTransition);
                    
                    EditorUtility.SetDirty(_currentStateMachine);

                    break;

                default:

                    Debug.LogError($"{type.Name} - {type.BaseType.Name} is not a valid type");
                    return;
            }
        }
        
        private StateMachineState CreateStateNodeView(State state)
        {
            var nodeView = new StateMachineState(state, _currentStateMachine);
            
            nodeView.Output.AddManipulator(new EdgeConnector<StateMachineTransition>(new StateMachineTransitionConnectorListener(OnEdgeDrop)));
            nodeView.Input.AddManipulator(new EdgeConnector<StateMachineTransition>(new StateMachineTransitionConnectorListener(OnEdgeDrop)));
            
            nodeView.StateSelected += () =>
            {
                NodeSelected?.Invoke(nodeView);
            };
            
            AddElement(nodeView);

            return nodeView;
        }

        private StateMachineTransition CreateTransitionEdgeView(Transition transition)
        {
            var edgeView = new StateMachineTransition(transition, _currentStateMachine);
            edgeView.TransitionSelected += () =>
            {
                EdgeSelected?.Invoke(edgeView);
            };

            // Debug.Log($"Created (Edge): {edgeView.GetType()}");
                        
            AddElement(edgeView);
            
            return edgeView;
        }
        
        private StateMachineTransition CreateTransitionEdgeView(StateMachineTransition edgeToCreate)
        {
            edgeToCreate.TransitionSelected += () =>
            {
                EdgeSelected?.Invoke(edgeToCreate);
            };

            // Debug.Log($"Created (StateMachineTransition): {edgeToCreate.GetType()}");
                        
            AddElement(edgeToCreate);

            ClearNonStateMachineTransitionEdges(); //workaround per evitare la duplicazione di edge sbagliati
            
            return edgeToCreate;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            var types = TypeCache.GetTypesDerivedFrom<State>();

            foreach (var t in types.Where(t => !t.IsAbstract))
            {
                evt.menu.AppendAction($"{t.BaseType?.Name}/{t.Name}", _ =>
                {
                    CreateElementByType(t);
                });
            }

            types = TypeCache.GetTypesDerivedFrom<Transition>();
            foreach (var t in types.Where(t => !t.IsAbstract)) 
            {
                evt.menu.AppendAction($"{t.BaseType?.Name}/{t.Name}", _ =>
                {
                    CreateElementByType(t);
                });
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }
    
        public void OnEdgeDrop(Edge edge)
        {
            // Debug.Log("OnEdgeDrop");

            var outputNode = edge.output.node as StateMachineState;
            var inputNode = edge.input.node as StateMachineState;
            
            if (outputNode != null 
                && inputNode != null)
            {
                var edgeToCreate = outputNode.Output.ConnectTo<StateMachineTransition>(inputNode.Input);
                
                var transition = new DefaultTransition()
                {
                    End = _currentStateMachine.StateMachine.States.IndexOf(inputNode.State),
                    Start = _currentStateMachine.StateMachine.States.IndexOf(outputNode.State)
                };

                edgeToCreate.transitionData = transition;

                _currentStateMachine.StateMachine.Transitions.Add(transition);

                CreateTransitionEdgeView(edgeToCreate);
            }
        }
        
        /// <summary>
        /// Clears all edges that are not of type StateMachineTransition.
        /// This is used when the user drags a new state machine onto the window.
        /// questa roba è un workaround perché, per qualche oscuro motivo, quando trasportavo
        /// l'edge e lo lo droppavo, mi creava due edge di cui uno sbagliato 
        /// </summary>
        public void ClearNonStateMachineTransitionEdges()
        {
            var edgesToRemove = edges.Where(edge => !(edge is StateMachineTransition)).ToList();
            
            foreach (var edge in edgesToRemove)
            {
                RemoveElement(edge);
            }
        }
    }
}