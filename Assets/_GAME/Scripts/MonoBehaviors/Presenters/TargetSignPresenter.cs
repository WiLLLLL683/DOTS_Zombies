using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TargetSignPresenter : MonoBehaviour
{
    [SerializeField] private TextSignView prefab;

    private EntityManager entityManager;
    private NativeArray<Target> targets;
    private NativeArray<LocalTransform> targetTransforms;
    private NativeArray<Entity> targetEntities;
    private List<TextSignView> views = new();

    private void Start()
    {
        GetEntities();

        for (int i = 0; i < targets.Length && i < targetTransforms.Length; i++)
        {
            float3 position = targetTransforms[i].Position;
            position.y = 0;
            TextSignView view = Instantiate(prefab, position, Quaternion.identity);
            views.Add(view);
        }
    }

    private void Update()
    {
        GetEntities();

        for (int i = 0; i < views.Count && i < targets.Length && i < targetTransforms.Length; i++)
        {
            int count = targets[i].count;

            views[i].SetText($"{count}");
            views[i].transform.position = targetTransforms[i].Position;
        }
    }

    private void GetEntities()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //получить цель перемещаемую игроком
        var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<Target>(), ComponentType.ReadOnly<LocalTransform>(), ComponentType.ReadOnly<MoveOnInput>());
        targets = query.ToComponentDataArray<Target>(Allocator.Temp);
        targetEntities = query.ToEntityArray(Allocator.Temp);
        targetTransforms = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
    }
}