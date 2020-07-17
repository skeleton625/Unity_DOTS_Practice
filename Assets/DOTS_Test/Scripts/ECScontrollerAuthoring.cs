using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.AI;
struct ControllerData : IComponentData
{
    public float lookSpeedH;
    public float lookSpeedV;
    public float zoomSpeed;
    public float dragSpeed;
    public float3 Position;
    public float3 Rotation;
}

class ECScontrollerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private float speedH;
    [SerializeField] private float speedV;
    [SerializeField] private float zoom;
    [SerializeField] private float drag;
    [SerializeField] private GameObject Follower;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        FollowEntity followEntity = Follower.GetComponent<FollowEntity>();

        if (followEntity == null)
            followEntity = Follower.AddComponent<FollowEntity>();
        followEntity.entityToFollow = entity;

        var controller = new ControllerData
        {
            lookSpeedH = speedH,
            lookSpeedV = speedV,
            zoomSpeed = zoom,
            dragSpeed = drag,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };

        dstManager.AddComponentData(entity, controller);
    }
}

class ECSController : ComponentSystem
{
    private float3 moveOffset = float3.zero;
    private float3 forward, right, up;
    private Vector3 rotateOffset = Vector3.zero;
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref ControllerData data) =>
        {
            if(Input.GetMouseButton(1))
            {
                rotateOffset = data.Rotation;
                rotateOffset.y += data.lookSpeedH * Input.GetAxis("Mouse X");
                rotateOffset.x -= data.lookSpeedV * Input.GetAxis("Mouse Y");
                Quaternion rotation = Quaternion.Euler(rotateOffset);
                forward = math.forward(rotation);
                right = math.forward(Quaternion.Euler(rotateOffset + Vector3.up * 90));
                up = math.forward(Quaternion.Euler(rotateOffset + Vector3.forward * 90));

                moveOffset = data.Position;
                float offsetDelta = UnityEngine.Time.deltaTime * data.dragSpeed;
                if (Input.GetKey(KeyCode.LeftShift)) offsetDelta *= 5.0f;
                if (Input.GetKey(KeyCode.W)) moveOffset += forward * offsetDelta;
                if (Input.GetKey(KeyCode.S)) moveOffset -= forward * offsetDelta;
                if (Input.GetKey(KeyCode.D)) moveOffset += right * offsetDelta;
                if (Input.GetKey(KeyCode.A)) moveOffset -= right * offsetDelta;
                if (Input.GetKey(KeyCode.Q)) moveOffset -= up * offsetDelta;
                if (Input.GetKey(KeyCode.E)) moveOffset += up * offsetDelta;

                EntityManager.SetComponentData(entity, new Rotation { Value = rotation });
                EntityManager.SetComponentData(entity, new Translation { Value = moveOffset });
                data.Position = moveOffset;
                data.Rotation = rotateOffset;
                EntityManager.SetComponentData(entity, data);
            }
        });
    }
}
