using UnityEngine;
using Unity.Entities;

public class PrefabConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject prefab;
    public static Entity prefabEntity;
    // IConvertGameObjectToEntity 인터페이스의 인터페이스 메소드
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        using (BlobAssetStore blobAssetStore = new BlobAssetStore())
        {
            Entity prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab,
                                  GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
            PrefabConverter.prefabEntity = prefabEntity;
        }
    }
}
