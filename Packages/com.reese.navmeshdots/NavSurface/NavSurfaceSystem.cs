using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using System.Collections.Concurrent;
using NavMeshDOTS.Components;

using RaycastHit = Unity.Physics.RaycastHit;
using BuildPhysicsWorld = Unity.Physics.Systems.BuildPhysicsWorld;

namespace Reese.NavMashDOTS
{
    [UpdateAfter(typeof(NavBasisSystem))]
    public class NavSurfaceSystem : SystemBase
    {
        static ConcurrentDictionary<int, bool> needsSurfaceDictionary = new ConcurrentDictionary<int, bool>();

        BuildPhysicsWorld buildPhysicsWorld => World.GetExistingSystem<BuildPhysicsWorld>();
        EntityCommandBufferSystem barrier => World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        protected override void OnUpdate()
        {
            var commandBuffer = barrier.CreateCommandBuffer().ToConcurrent();
            var defaultBasis = World.GetExistingSystem<NavBasisSystem>().DefaultBasis;

            Entities
                .WithNone<Parent>()
                .ForEach((Entity entity, int entityQueryIndex, in NavSurface surface) =>
                {
                    if (surface.Basis.Equals(Entity.Null))
                        commandBuffer.AddComponent(entityQueryIndex, entity, new Parent { Value = defaultBasis });
                    else
                        commandBuffer.AddComponent(entityQueryIndex, entity, new Parent { Value = surface.Basis });

                    commandBuffer.AddComponent<LocalToParent>(entityQueryIndex, entity);
                })
                .WithoutBurst()
                .WithName("NavAddParentToSurfaceJob")
                .ScheduleParallel();

            barrier.AddJobHandleForProducer(Dependency);

            Entities
                .WithNone<Parent>()
                .ForEach((Entity entity, int entityInQueryIndex, in NavAgent agent) =>
                {
                    commandBuffer.AddComponent<Parent>(entityInQueryIndex, entity);
                    commandBuffer.AddComponent<LocalToParent>(entityInQueryIndex, entity);
                })
                .WithoutBurst()
                .WithName("NavAddParentToAgentJob")
                .ScheduleParallel();

            barrier.AddJobHandleForProducer(Dependency);

            Entities
                .WithAll<CompositeScale>()
                .WithAny<NavSurface, NavBasis>()
                .ForEach((Entity entity, int entityInQueryIndex) =>
                {
                })
                .WithName("NavRemoveCompositeScaleJob")
                .ScheduleParallel();

            barrier.AddJobHandleForProducer(Dependency);
        }
    }
}

