using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public class EntitySpawnSystem : ComponentSystem
{
    private int cnt = 0;
    protected override void OnCreate()
    {

    }

    protected override void OnUpdate()
    {
        if (cnt < 1000)
        {
            Entity spawnedEntity = EntityManager.Instantiate(PrefabConverter.prefabEntity);

            float3 pos = UnityEngine.Random.insideUnitSphere * 64;
            pos.y = 0;

            EntityManager.SetComponentData(spawnedEntity, new Translation
            {
                Value = pos
            });
            ++cnt;
        }
    }
}
