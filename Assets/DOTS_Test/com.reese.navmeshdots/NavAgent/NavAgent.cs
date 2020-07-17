using Unity.Entities;
using Unity.Mathematics;

namespace Reese.NavMashDOTS
{
    public class NavAgent : IComponentData
    {
        public float3 Offset;
        public float TranslationSpeed;
        public int SurfaceRaycastCount;

        public int TypeID;
        public int PathBufferIndex;
        public float3 Destination;
    }
}

