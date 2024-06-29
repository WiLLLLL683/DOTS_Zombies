using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

public class GoalSignPresenter : MonoBehaviour
{
    [SerializeField] private TextSignView prefab;

    private EntityManager entityManager;
    private NativeArray<GoalTrigger> goals;
    private NativeArray<LocalTransform> goalTransforms;
    private NativeArray<Entity> goalEntities;
    private List<TextSignView> views = new();

    private void Start()
    {
        GetEntities();

        for (int i = 0; i < goals.Length && i < goalTransforms.Length; i++)
        {
            float3 position = goalTransforms[i].Position;
            position.y = 0;
            TextSignView view = GameObject.Instantiate(prefab, position, Quaternion.identity);
            views.Add(view);
        }
    }

    private void Update()
    {
        GetEntities();

        for (int i = 0; i < views.Count && i < goals.Length && i < goalEntities.Length; i++)
        {
            string name = entityManager.GetName(goalEntities[i]);
            int count = goals[i].count;
            int countRequired = goals[i].countRequired;
            bool isComplete = goals[i].isComplete;

            views[i].SetText($"{name}: {count}/{countRequired}");

            if (isComplete)
                views[i].SetColor(Color.green);
            else
                views[i].SetColor(Color.white);
        }
    }

    //private void OnDestroy()
    //{
    //    for (int i = 0; i < views.Count; i++)
    //    {
    //        GameObject.Destroy(views[i].gameObject);
    //    }

    //    views.Clear();
    //}

    private void GetEntities()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //получить goals
        var goalQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<GoalTrigger>(), ComponentType.ReadOnly<LocalTransform>());
        goals = goalQuery.ToComponentDataArray<GoalTrigger>(AllocatorManager.Temp);
        goalEntities = goalQuery.ToEntityArray(AllocatorManager.Temp);
        goalTransforms = goalQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
    }
}
