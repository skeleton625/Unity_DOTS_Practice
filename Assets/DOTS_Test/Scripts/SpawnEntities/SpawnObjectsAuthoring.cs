using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using System.Collections.Generic;

interface ISpawnSettings
{
    Entity Prefab { get; set; }
    float3 Position { get; set; }
    int Range { get; set; }
    int Count { get; set; }
}

struct SpawnSettings : IComponentData, ISpawnSettings
{
    public Entity Prefab { get; set; }
    public float3 Position { get; set; }
    public int Range { get; set; }
    public int Count { get; set; }
}

abstract class SpawnObjectAuthoringBase<T> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    where T : struct, IComponentData, ISpawnSettings
{
    [Header("Default Spawn Settings")]
    [SerializeField] protected GameObject objectPrefab = null;
    [SerializeField] protected int generateRange = 0;
    [SerializeField] protected int objectCount = 0;

    public virtual void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem){ }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) => referencedPrefabs.Add(objectPrefab);
}

abstract class SpawnObjectsSystemBase<T> : ComponentSystem
    where T : struct, IComponentData, ISpawnSettings
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref T spawnSettings) =>
        {
            var count = spawnSettings.Count;

            OnBeforeInstantiatePrefab(ref spawnSettings);

            var instances = new NativeArray<Entity>(count, Allocator.Temp);
            EntityManager.Instantiate(spawnSettings.Prefab, instances);

            var positions = new NativeArray<float3>(count, Allocator.Temp);
            var rotations = new NativeArray<quaternion>(count, Allocator.Temp);
            SetEntitiesTransforms(ref positions, ref rotations, spawnSettings);

            for (int i = 0; i < count; i++)
            {
                var instance = instances[i];
                EntityManager.SetComponentData(instance, new Translation { Value = positions[i] });
                EntityManager.SetComponentData(instance, new Rotation { Value = rotations[i] });
                ConfigureInstance(ref instance, spawnSettings);
            }

            EntityManager.RemoveComponent<T>(entity);
        });
    }

    protected virtual void OnBeforeInstantiatePrefab(ref T spawnSettings) { }
    protected virtual void ConfigureInstance(ref Entity entity, T spawnSettings) { }
    protected abstract void SetEntitiesTransforms(ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, T spanwSettings);
}

