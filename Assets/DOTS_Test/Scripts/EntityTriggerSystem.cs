using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class EntityTriggerSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct EntityTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<TreeData> allTrees;
        [ReadOnly] public ComponentDataFromEntity<ControllerData> allPlayers;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            if (allTrees.Exists(entityA) && allTrees.Exists(entityB))
                return;
            if(allTrees.Exists(entityA) && allPlayers.Exists(entityB))
            {
                Debug.Log(entityA.ToString() + " " + entityB.ToString());
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if(allPlayers.Exists(entityA) && allTrees.Exists(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityB);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new EntityTriggerSystemJob();

        job.allTrees = GetComponentDataFromEntity<TreeData>(true);
        job.allPlayers = GetComponentDataFromEntity<ControllerData>(true);
        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        commandBufferSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
