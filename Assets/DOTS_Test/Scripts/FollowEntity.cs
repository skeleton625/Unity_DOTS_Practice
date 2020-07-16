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

        Translation entPos = Emanager.GetComponentData<Translation>(entityToFollow);
        Rotation entRot = Emanager.GetComponentData<Rotation>(entityToFollow);
        transform.position = entPos.Value + offset;
        transform.rotation = entRot.Value;
    }
}
