using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StateMachineSpace
{
    [Serializable]
    public abstract class Transition
    {
        public int Start;
        public int End;
        public int Priority = 0;

        public abstract bool ShouldTransition(StateMachine stateMachine, GameObject agent);

        public virtual UniTask OnTransition()
        {
            return UniTask.CompletedTask;
        }

        public Transition Clone(GameObject agent)
        {
            var result = Activator.CreateInstance(GetType()) as Transition;
            OnClone(ref result, agent);
            return result;
        }
        
        public virtual void OnClone(ref Transition newObject, GameObject agent)
        {
            newObject.Start = Start;
            newObject.End = End;
        }
    }
}