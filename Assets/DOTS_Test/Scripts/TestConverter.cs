using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class TestConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        int num;
        if (!Int32.TryParse(transform.parent.name, out num))
            num = 0;

        var data = new CitizenData
        {
            CitizenNumber = num
        };

        dstManager.AddComponentData(entity, data);
    }
}
