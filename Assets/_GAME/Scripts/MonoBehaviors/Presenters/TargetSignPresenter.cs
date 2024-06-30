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

        var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<Target>(), ComponentType.ReadOnly<LocalTransform>());
        targets = query.ToComponentDataArray<Target>(AllocatorManager.Temp);
        targetEntities = query.ToEntityArray(AllocatorManager.Temp);
        targetTransforms = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
    }
}