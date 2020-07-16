using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class SpawnCitizenAuthoring : SpawnObjectAuthoringBase<SpawnSettings>
{
    public override void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var settings = new SpawnSettings
        {
            Prefab = conversionSystem.GetPrimaryEntity(objectPrefab),
            Position = transform.position,
            Range = generateRange,
            Count = objectCount
        };

        dstManager.AddComponentData(entity, settings);
    }
}
