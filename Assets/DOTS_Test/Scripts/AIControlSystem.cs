using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

public class AIControlSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            ObjectManager Omanager = GameObject.FindObjectOfType<ObjectManager>();
            Entities
                .ForEach((Entity entity, ref CitizenData data) =>
                 {
                     if (data.CitizenNumber < 0) return;

                     Omanager.agents[data.CitizenNumber].SetDestination(new Vector3(-100, 0, -100));
                 });
        }
    }
}
