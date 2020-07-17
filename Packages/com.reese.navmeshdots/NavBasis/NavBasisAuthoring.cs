using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using NavMeshDOTS.Components;

namespace Reese.NavMashDOTS
{
    public class NavBasisAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {

        public bool HasGameObjectTransform;
        public NavBasisAuthoring ParentBasis;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new NavBasis { ParentBasis = conversionSystem.GetPrimaryEntity(ParentBasis) });

            if (HasGameObjectTransform) dstManager.AddComponent(entity, typeof(CopyTransformFromGameObject));
            else dstManager.AddComponent(entity, typeof(CopyTransformToGameObject));

            dstManager.RemoveComponent(entity, typeof(NonUniformScale));
            dstManager.RemoveComponent(entity, typeof(MeshRenderer));
        }
    }
}
