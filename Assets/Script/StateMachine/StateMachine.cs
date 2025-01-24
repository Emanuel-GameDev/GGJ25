using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using UnityEngine;

namespace StateMachineSpace
{
    [Serializable]
    public class StateMachine
    {
        [SerializeField] private int DefaultState = 0;

        [SerializeReference, SubclassSelector]
        public List<State> States;
        
        [SerializeReference, SubclassSelector]
        public List<Transition> Transitions;

        private int _currentState = 0;
        public int CurrentState => _currentState;

        [BurstCompile]
        public StateMachine Clone(GameObject target)
        {
            var states = new List<State>();

            for (var i = 0; i < States.Count; i++)
            {
                states.Add(States[i].Clone(target));
            }

            var transitions = new List<Transition>();
            for (var i = 0; i < Transitions.Count; i++)
            {
                transitions.Add(Transitions[i].Clone(target));
            }

            return new StateMachine { States = states, Transitions = transitions };
        }

        [BurstCompile]
        public async UniTask Init(GameObject target, CancellationToken token)
        {
            _currentState = DefaultState;
            await States[_currentState].OnEnter(target);
            await Update(target, token);
        }

        [BurstCompile]
        public async UniTask Update(GameObject target, CancellationToken token)//TODO cancellation token per interrompere lo stato e override dello stato corrente
        {
            while (true && !token.IsCancellationRequested)
            {
                if (ShouldExecuteTransition(target, out var transition))
                {
                    await PerformTransition(transition, target);
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }

                await States[_currentState].OnUpdate(target);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        [BurstCompile]
        private bool ShouldExecuteTransition(GameObject target, out int selectedTransition)
        {
            selectedTransition = 0;
            var possibleTransitions = Transitions.Where(x => x.Start == _currentState && x.ShouldTransition(this, target)).ToList();

            if (!possibleTransitions.Any())
            {
                return false;
            }
            
            possibleTransitions.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            selectedTransition = Transitions.IndexOf(possibleTransitions.First());
            return true;
        }

        [BurstCompile]
        private async UniTask PerformTransition(int transition, GameObject target)
        {
            await States[_currentState].OnExit(target);
            await Transitions[transition].OnTransition();
            _currentState = Transitions[transition].End;
            await States[_currentState].OnEnter(target);
        }
    }
}