using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using NavMeshJob.Components;

namespace NavMeshJob.System
{
    public class NavAgentSystem : JobComponentSystem
    {
        private struct AgentData
        {
            public int index;
            public Entity entity;
            public NavAgent agnet;
        }

        // 아직 구현 안됨
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle job = new JobHandle();
            return job;
        }
    }
}