using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using NavMeshDOTS.Components;

namespace Reese.NavMashDOTS
{
    public class NavSurfaceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public bool HasGameObjectTransform;
        public NavBasisAuthoring Basis;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (HasGameObjectTransform) dstManager.AddComponent(entity, typeof(CopyTransformFromGameObject));
            else dstManager.AddComponent(entity, typeof(CopyTransformToGameObject));

            dstManager.AddComponentData(entity, new NavSurface { Basis = conversionSystem.GetPrimaryEntity(Basis) });
            dstManager.RemoveComponent(entity, typeof(NonUniformScale));
            dstManager.RemoveComponent(entity, typeof(Translation));
            dstManager.RemoveComponent(entity, typeof(Rotation));
            dstManager.RemoveComponent(entity, typeof(MeshRenderer));
            dstManager.RemoveComponent(entity, typeof(RenderMesh));
        }
    }
}
