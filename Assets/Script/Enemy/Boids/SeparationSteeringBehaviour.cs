using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Boids
{
    [Serializable]
    public class SeparationSteeringBehaviour : NeighbourBasedSteeringBehaviour
    {
        [SerializeField]
        private float _maxAcceleration = 100;

        [SerializeField]
        private AnimationCurve _forceDropCurve;
        
        [SerializeField]
        private float _arriveDistance;

        [BurstCompile]
        protected override SteeringOutput GetSteeringForNeighbours(Agent agent, Agent[] neighbours)
        {
            if(math.distance(agent.transform.position, BubbleController.Instance.transform.position) < _arriveDistance)
            {
                return new SteeringOutput { Linear = 0, Angular = 0 };
            }

            var targetVelocity = float3.zero;
            foreach (var neighbour in neighbours)
            {
                var distance = math.distance(agent.Position, neighbour.Position);
                var forceFactor = distance / NeighbourRange;
                var force = _forceDropCurve.Evaluate(forceFactor) * math.normalizesafe(agent.Position - neighbour.Position);
                targetVelocity += force;
                // Debug.DrawRay(agent.Position, force, Color.yellow);
            }

            targetVelocity /= math.max(neighbours.Length, 1);
            // Debug.DrawRay(agent.Position, targetVelocity, Color.green);
            targetVelocity *= _maxAcceleration;

            return new SteeringOutput { Linear = targetVelocity, Angular = 0 };
        }
    }
}