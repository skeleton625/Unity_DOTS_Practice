using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int xSize, ySize, zSize;
    [Range(0.1f, 2f), SerializeField] private float spacing;
    [SerializeField] private GameObject gameObjectPrefab;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager Emanager;
    
    // Conversion Workflow Code
    private void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        Emanager = defaultWorld.EntityManager;

        /* var */
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        InstantiateEntityGrid(xSize, ySize, zSize, spacing);
    }

    private void InstantiateEntity(float3 position)
    {
        Entity cloneEntity = Emanager.Instantiate(entityPrefab);

        Emanager.SetComponentData(cloneEntity, new Translation
        {
            Value = position
        });
        Emanager.SetComponentData(cloneEntity, new UnitData
        {
            UnitType = 1
        });
    }

    private void InstantiateEntityGrid(int dimX, int dimY, int dimZ, float spacing = 1f)
    {
        for(int i = 0; i < dimX; i++)
            for (int j = 0; j < dimY; j++)
                for(int k = 0; k < dimZ; k++)
                    InstantiateEntity(new float3(i * spacing, j * spacing, k * spacing));
    }
    

    /*
     * 
     * Pure ECS Code
     * 
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;

    private void MakeEntity()
    {
        EntityManager Emanager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = Emanager.CreateArchetype(
            typeof(Translation),
            typeof(Scale),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );
        Entity myEntity = Emanager.CreateEntity(archetype);
        Emanager.AddComponentData(myEntity, new Translation 
        { 
            Value = new float3 ( 2f, 0f, 4f )
        });
        Emanager.AddComponentData(myEntity, new Scale
        {
            Value = 1f
        });
        Emanager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = unitMesh,
            material = unitMaterial
        });
    }
    */
}
