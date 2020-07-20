using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectManager : MonoBehaviour
{
    [Header("Object Settings")]
    [SerializeField] private GameObject ObjectModel;
    [SerializeField] private int ObjectCount;

    [HideInInspector] public GameObject[] objects;
    [HideInInspector] public NavMeshAgent[] agents;

    private void Start()
    {
        objects = new GameObject[ObjectCount];
        agents = new NavMeshAgent[ObjectCount];

        for (int i = 0; i < ObjectCount; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 100;
            pos.y = 1;
            objects[i] = Instantiate(ObjectModel, pos, Quaternion.identity);
            objects[i].name = i.ToString();

            agents[i] = objects[i].GetComponent<NavMeshAgent>();
        }
    }
}
