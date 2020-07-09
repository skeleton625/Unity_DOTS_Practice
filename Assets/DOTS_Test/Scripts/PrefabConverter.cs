using UnityEngine;
using Unity.Entities;

public class PrefabConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject[] prefabs;
    public static Entity[] prefabEntities;
    // IConvertGameObjectToEntity 인터페이스의 인터페이스 메소드

    private void Awake()
    {
        prefabEntities = new Entity[prefabs.Length];
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        using (BlobAssetStore blobAssetStore = new BlobAssetStore())
        {
            for(int i = 0; i < prefabs.Length; i++)
            {
                Entity prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabs[i],
                      GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
                PrefabConverter.prefabEntities[i] = prefabEntity;
            }
        }
    }
}
