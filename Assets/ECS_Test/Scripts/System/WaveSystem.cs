using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/* Comopnent로 추가할 필요 없음 */
public class WaveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        /* 모든 Entity 들에 대한 함수 */
        Entities.ForEach((ref Translation trans, ref UnitData data) =>
        {
            float zPos = data.amplitude * math.sin((float)(Time.ElapsedTime * data.UnitSpeed +
                                                             trans.Value.x * data.xOffset + trans.Value.y * data.yOffset));
            trans.Value = new float3(trans.Value.x, trans.Value.y, zPos);
        });
    }
}
