using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow = Entity.Null;
    public float3 offset = float3.zero;
    private EntityManager Emanager;

    private void Awake()
    {
        Emanager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if (entityToFollow == Entity.Null) return;

        Emanager.SetComponentData(entityToFollow, new Translation { Value = transform.position });
        Emanager.SetComponentData(entityToFollow, new Rotation { Value = transform.rotation });
    }
}
