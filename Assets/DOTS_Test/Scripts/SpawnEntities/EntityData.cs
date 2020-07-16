
using UnityEngine;
using Unity.Entities;
using Unity.Physics;

[GenerateAuthoringComponent]
public struct EntityData : IComponentData
{
    public int typeNumber;
    public Entity prefabEntity;
}