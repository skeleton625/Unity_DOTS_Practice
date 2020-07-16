using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace NavMeshJob.Components
{
    public enum AgentStatus
    {
        Idle = 0,
        PathQueued = 1,
        Moving = 2,
        Paused = 4
    }

    [GenerateAuthoringComponent]
    public struct NavAgent : IComponentData
    {
        public float StoppingDistance;
        public float MoveSpeed;
        public float Acceleration;
        public float RotationSpeed;
        public int AreaMask;

        public float3 destination { get; set; }
        public float currentMoveSpeed { get; set; }
        public int queryVersion { get; set; }
        public AgentStatus status { get; set; }
        public float3 position { get; set; }
        public float3 nextPosition { get; set; }
        public Quaternion rotation { get; set; }
        public float remainingDistance { get; set; }
        public float3 currentWaypoint { get; set; }
        public int nextWaypointIndex { get; set; }
        public int totalWaypoints { get; set; }
    }

}