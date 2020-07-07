using Unity.Entities;

[GenerateAuthoringComponent]
public struct UnitData : IComponentData
{
    public int UnitType;
    public float UnitSpeed;
    public float amplitude;
    public float xOffset, yOffset;
}