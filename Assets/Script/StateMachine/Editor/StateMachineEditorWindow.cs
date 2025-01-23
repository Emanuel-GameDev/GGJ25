using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateMachineSpace.Editor
{
    public class StateMachineEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        private StateMachineSchema _currentStateMachine;

        [MenuItem("Tools/State Machine Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<StateMachineEditorWindow>();
            window.titleContent = new GUIContent("State Machine Editor");
        }

        public void CreateGUI()
        {
            _visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Script/StateMachine/Editor/StateMachineEditorWindow.uxml");
            
            ClearView();
            
            _visualTreeAsset.CloneTree(rootVisualElement);
            
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is StateMachineSchema stateMachine)
            {
                if (stateMachine == _currentStateMachine)
                {
                    return;
                }

                ClearView();
                _currentStateMachine = stateMachine;
                PopulateView();
            }
            else
            {
                ClearView();
            }
        }

        private void PopulateView()
        {
            // Debug.Log("Populate view Editor");
            if (_currentStateMachine == null)
            {
                return;
            }

            var serializedObjectTree = new SerializedObject(_currentStateMachine);
            rootVisualElement.Bind(serializedObjectTree);

            var label = rootVisualElement.Q<Label>("state-machine-name");
            if (label != null)
            {
                label.text = _currentStateMachine.name;
            }
            
            var graphView = rootVisualElement.Q<StateMachineGraphView>();

            graphView.PopulateView(_currentStateMachine);
            
            // //TODO: remove
            // graphView.ElementRemoved -= OnNodeSelected;
            // graphView.ElementRemoved += OnNodeSelected;

            graphView.NodeSelected -= OnNodeSelected;
            graphView.NodeSelected += OnNodeSelected;

            graphView.EdgeSelected -= OnEdgeSelected;
            graphView.EdgeSelected += OnEdgeSelected;
        }

        private void OnNodeSelected(StateMachineState node)
        {
            // Debug.Log("Node selected: " + node);

            var serializedObjectTree = new SerializedObject(_currentStateMachine);

            int indexOfNode = 0;
            if(_currentStateMachine.StateMachine.States.Contains(node.State))
            {
                indexOfNode = _currentStateMachine.StateMachine.States.IndexOf(node.State);
            }

            var serializedProp = serializedObjectTree.FindProperty("StateMachine");

            var serializedPropRel = serializedProp.FindPropertyRelative("States");

            var serializedElement = serializedPropRel.GetArrayElementAtIndex(indexOfNode);

            rootVisualElement.Q<PropertyField>("inspector").BindProperty(serializedElement);
        }

        private void OnEdgeSelected(StateMachineTransition edge)
        {
            // Debug.Log("Edge selected: " + edge);

            var serializedObjectTree = new SerializedObject(_currentStateMachine);

            var findProp =  serializedObjectTree
                                .FindProperty("StateMachine");

            var findRelativeProp = findProp.FindPropertyRelative("Transitions");

            serializedObjectTree.Update();

            int indexOfEdge = 0;
            if(_currentStateMachine.StateMachine.Transitions.Contains(edge.transitionData))
            {
                indexOfEdge = _currentStateMachine.StateMachine.Transitions.IndexOf(edge.transitionData);
            }

            serializedObjectTree.Update();

            var serializedEdge = findRelativeProp
                                    .GetArrayElementAtIndex(indexOfEdge);

            if (serializedEdge == null)
            {
                return;
            }

            rootVisualElement.Q<PropertyField>("inspector").BindProperty(serializedEdge);
        }

        private void ClearView()
        {
            if (_currentStateMachine == null)
            {
                return;
            }

            rootVisualElement.Unbind();
            var label = rootVisualElement.Q<Label>("state-machine-name");
            if (label != null)
            {
                label.text = string.Empty;
            }

            _currentStateMachine = null;
        }

        private void UnbindInspector()
        {
            rootVisualElement.Unbind();
        }
    }
}