using Unity.Entities;

[GenerateAuthoringComponent]
public struct EntityData : IComponentData
{
    public int typeNumber;
    public Entity prefabEntity;
}
