using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;

public class EntityRaycastController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayRange;
    private BuildPhysicsWorld buildPhysicsWorld;
    private CollisionWorld collisionWorld;
    private CollisionFilter raycastFilter;

    private void Start()
    {
        raycastFilter = new CollisionFilter
        {
            BelongsTo = ~0u,
            CollidesWith = ~0u,
            GroupIndex = 0
        };
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            UnityEngine.Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Entity entity = EntityRaycast(ray.origin, ray.direction * rayRange);
            if (entity != Entity.Null)
                Debug.Log(entity);
        }
    }

    private Entity EntityRaycast(float3 from, float3 to)
    {
        buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
        RaycastInput raycastInput = new RaycastInput
        {
            Start = from,
            End = to,
            Filter = raycastFilter
        };

        Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();

        if(collisionWorld.CastRay(raycastInput, out raycastHit))
        {
            Entity hitEntity = buildPhysicsWorld.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            return hitEntity;
        } else
        {
            return Entity.Null;
        }
    }
}
