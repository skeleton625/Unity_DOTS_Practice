using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class EntitySpawner : MonoBehaviour
{
    [Header("Entity Attribute")]
    [SerializeField] private int Count;
    [SerializeField] private float scale;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    [SerializeField] private float SpawnRadius;

    private EntityManager Emanager;

    private void Start()
    {
        SpawnEntityPure(Count);
    }

    private EntityArchetype SetupEntityPure()
    {
        World defaultWorld = World.DefaultGameObjectInjectionWorld;
        Emanager = defaultWorld.EntityManager;

        EntityArchetype archetype = Emanager.CreateArchetype
            (
                typeof(Translation),
                typeof(Rotation),
                typeof(Scale),
                typeof(LocalToWorld),
                typeof(RenderBounds),
                typeof(RenderMesh)
            );
        return archetype;
    }

    private void SpawnEntityPure(int Count)
    {
        EntityArchetype entityArchetype = SetupEntityPure();

        for (int i = 0; i < Count; i++)
        {
            Entity entity = Emanager.CreateEntity(entityArchetype);

            Emanager.AddComponentData(entity, new Translation { Value = GetRandomPosition(SpawnRadius) });
            Emanager.AddComponentData(entity, new Rotation { Value = Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0) });
            Emanager.AddComponentData(entity, new Scale { Value = scale });

            Emanager.AddSharedComponentData(entity, new RenderMesh
            {
                mesh = this.mesh,
                material = this.material
            });
        }
    }

    private float3 GetRandomPosition(float radius)
    {
        return transform.position + UnityEngine.Random.insideUnitSphere * radius;
    }
}
