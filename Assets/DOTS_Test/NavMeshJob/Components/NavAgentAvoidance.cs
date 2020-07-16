
using Unity.Entities;
using Unity.Mathematics;

namespace NavMeshJob.Components
{
    [GenerateAuthoringComponent]
    public struct NavAgentAvoidance : IComponentData
    {
        public float radius;
        public float3 partition { get; set; }
    }
}
