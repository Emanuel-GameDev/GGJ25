using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Boids
{
    [Serializable]
    public abstract class NeighbourBasedSteeringBehaviour : SteeringBehaviour
    {
        [SerializeField]
        protected float NeighbourRange = 10;

        [SerializeField]
        private float _visionConeSize = 60;
        
        public sealed override SteeringOutput GetSteering(Agent agent)
        {
            return GetSteeringForNeighbours(agent, FindNeighbours(agent));
        }

        private Agent[] FindNeighbours(Agent agent)
        {
            //return Object.FindObjectsOfType<Agent>().Where(x => x != agent && math.distancesq(agent.Position, x.Position) <= NeighbourRange * NeighbourRange).ToArray();

            var results = new List<Agent>();
            var possibleNeighbours = Object.FindObjectsOfType<Agent>().Where(x => x != agent && math.distancesq(agent.Position, x.Position) <= NeighbourRange * NeighbourRange).ToArray();
            foreach (var possibleNeighbour in possibleNeighbours)
            {
                var directionToNeighbour = math.normalizesafe(possibleNeighbour.Position - agent.Position);
                var orientationToNeighbour = MapToRange(math.atan2(directionToNeighbour.x, directionToNeighbour.z));

                if (orientationToNeighbour >= agent.Orientation - math.radians(_visionConeSize) &&
                    orientationToNeighbour <= agent.Orientation + math.radians(_visionConeSize))
                {
                    results.Add(possibleNeighbour);
                }
            }

            return results.ToArray();
        }

        protected abstract SteeringOutput GetSteeringForNeighbours(Agent agent, Agent[] neighbours);
        
        private float MapToRange(float angle)
        {
            const float twoPi = 2 * math.PI;
            while (angle < 0)
            {
                angle += twoPi;
            }

            while (angle > twoPi)
            {
                angle -= twoPi;
            }

            return angle;
        }
    }
}