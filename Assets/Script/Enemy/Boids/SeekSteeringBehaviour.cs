using System;
using Unity.Mathematics;
using UnityEngine;

namespace Boids
{
    [Serializable]
    public class SeekSteeringBehaviour : SteeringBehaviour
    {
        [SerializeField]
        private float _maxAcceleration = 100;
        [SerializeField]
        private float _arriveDistance = 1;

        [SerializeField]
        private Transform _target;

        public sealed override SteeringOutput GetSteering(Agent agent)
        {
            _target = BubbleController.Instance.transform;

            //Debug.Log("distance: " + math.distance(agent.transform.position, _target.position));
            if(math.distance(agent.transform.position, _target.position) > _arriveDistance)
            {
                return GetSteeringForTargetPosition(agent, _target.position);
            }

            return new SteeringOutput{Linear = 0, Angular = 0};
        }

        protected virtual SteeringOutput GetSteeringForTargetPosition(Agent agent, float3 target)
        {
            return new SteeringOutput
            {
                Linear = math.normalizesafe(target - agent.Position) * _maxAcceleration,
                Angular = 0
            };
        }
    }
}