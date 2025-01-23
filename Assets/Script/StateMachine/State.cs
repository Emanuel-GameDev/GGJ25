using System;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace StateMachineSpace
{
    [Serializable]
    public abstract class State
    {
        public bool IsEnded { get; protected set; } = false;
        [HideInInspector] public float2 GraphPosition;
        
        [BurstCompile]
        public virtual UniTask OnEnter(GameObject agent)
        {
            IsEnded = false;
            return UniTask.CompletedTask;
        } 

        [BurstCompile]
        public virtual UniTask OnUpdate(GameObject agent) 
        {
            return UniTask.CompletedTask;
        }

        [BurstCompile]
        public virtual UniTask OnExit(GameObject agent)
        {
            return UniTask.CompletedTask;
        }
        
        public State Clone(GameObject agent)
        {
            var result = Activator.CreateInstance(GetType()) as State;
            OnClone(ref result, agent);
            return result;
        }
        
        [BurstCompile]
        public virtual void OnClone(ref State newObject, GameObject agent)
        {
        }
    }
}