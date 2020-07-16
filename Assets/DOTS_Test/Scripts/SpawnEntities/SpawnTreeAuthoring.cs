using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Collections;
using Erandom = UnityEngine.Random;

struct TreeSpawnSettings : IComponentData, ISpawnSettings
{
    public Entity Prefab { get; set; }
    public float3 Position { get; set; }
    public int Range { get; set; }
    public int Count { get; set; }
    public int TreeType { get; set; }
}

class SpawnTreeAuthoring : SpawnObjectAuthoringBase<TreeSpawnSettings>
{
    [Header("Extra Tree Settings")]
    [SerializeField] private int treeType = 0;

    public override void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        ChangePositionByTypes();

        var settings = new TreeSpawnSettings
        {
            Prefab = conversionSystem.GetPrimaryEntity(objectPrefab),
            Position = transform.position,
            Range = generateRange,
            Count = objectCount,
            TreeType = treeType
        };

        dstManager.AddComponentData(entity, settings);
    }

    private void ChangePositionByTypes()
    { }
}

class SpawnTreeSystem : SpawnObjectsSystemBase<TreeSpawnSettings>
{
    protected override void SetEntitiesTransforms(ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, TreeSpawnSettings spawnSettings)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            rotations[i] = Quaternion.Euler(-90, Erandom.Range(0, 360), 0);

            float3 pos = spawnSettings.Position + (float3)Erandom.insideUnitSphere * spawnSettings.Range;
            pos.y = 0;
            positions[i] = pos;
        }
    }
}