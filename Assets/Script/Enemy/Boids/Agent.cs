using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Boids
{
    public class Agent : MonoBehaviour
    {
        [SerializeField]
        private float _maxLinearSpeed = 10;

        [SerializeField]
        private float _maxAngularSpeed = 360f;

        [SerializeField]
        private float _radius = .5f;

        [SerializeReference, SubclassSelector]
        private SteeringBehaviour[] _steering = Array.Empty<SteeringBehaviour>();

        public float MaxLinearSpeed => _maxLinearSpeed;
        public float MaxAngularSpeed => _maxAngularSpeed;
        public float Radius => _radius;

        public float3 LinearVelocity { get; private set; }
        public float AngularVelocity { get; private set; }

        public float3 Position
        {
            get => transform.position;
            private set => transform.position = value;
        }

        public float Orientation
        {
            get => math.radians(transform.eulerAngles.y);
            private set => transform.rotation = quaternion.Euler(0, value, 0);
        }

        [BurstCompile]
        private void FixedUpdate()
        {
            foreach (var currentSteering in _steering)
            {
                var steering = currentSteering.GetSteering(this);
                LinearVelocity += steering.Linear * Time.fixedDeltaTime;
                AngularVelocity += steering.Angular * Time.fixedDeltaTime;
            }

            LinearVelocity = math.normalizesafe(LinearVelocity) * math.min(math.length(LinearVelocity), MaxLinearSpeed);
            AngularVelocity = math.clamp(math.abs(AngularVelocity), 0, MaxAngularSpeed) * math.sign(AngularVelocity);

            LinearVelocity = new float3(LinearVelocity.x, LinearVelocity.y, 0f);

            Position += LinearVelocity * Time.fixedDeltaTime;
            Orientation += AngularVelocity * Time.fixedDeltaTime;

            // while (Position.x > 50)
            // {
            //     Position = new float3(Position.x - 100, 0, 0);
            // }
            
            // while (Position.x < -50)
            // {
            //     Position = new float3(Position.x + 100, 0, 0);
            // }
            
            // while (Position.z > 50)
            // {
            //     Position = new float3(0, 0, Position.z - 100);
            // }
            
            // while (Position.z < -50)
            // {
            //     Position = new float3(0, 0, Position.z + 100);
            // }
        }
    }
}