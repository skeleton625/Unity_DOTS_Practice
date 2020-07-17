using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Reese.NavMashDOTS
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public class NavBasisSystem : SystemBase
    {
        public Entity DefaultBasis { get; private set; }
        EntityCommandBufferSystem barrier => World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        protected override void OnCreate()
        {
            DefaultBasis = World.EntityManager.CreateEntity();
            World.EntityManager.AddComponent(DefaultBasis, typeof(NavBasis));
            World.EntityManager.AddComponent(DefaultBasis, typeof(LocalToWorld));
            World.EntityManager.AddComponent(DefaultBasis, typeof(Translation));
            World.EntityManager.AddComponent(DefaultBasis, typeof(Rotation));
        }

        protected override void OnUpdate()
        {
            var commandBuffer = barrier.CreateCommandBuffer().ToConcurrent();
            Entities
                .WithNone<Parent>()
                .ForEach((Entity entity, int entityInQueryIndex, in NavBasis basis) =>
                {
                    if (basis.ParentBasis.Equals(Entity.Null)) return;

                    commandBuffer.AddComponent(entityInQueryIndex, entity, new Parent { Value = basis.ParentBasis });
                    commandBuffer.AddComponent(entityInQueryIndex, entity, typeof(LocalToParent));
                })
                .WithoutBurst()
                .WithName("NavAddParentToBasisJob")
                .ScheduleParallel();

            barrier.AddJobHandleForProducer(Dependency);
        }
    }
}
