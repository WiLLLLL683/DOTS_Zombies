using System;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer radius;

    private void Update()
    {
        //получить query
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<Target>(), ComponentType.ReadOnly<LocalTransform>(), ComponentType.ReadOnly<MoveOnInput>());

        if (query.IsEmpty)
            return;

        //в мире должна быть одна сущность с Target и MoveOnInput
        var target = query.ToComponentDataArray<Target>(Allocator.Temp)[0];
        var targetEntity = query.ToEntityArray(Allocator.Temp)[0];
        var targetTransform = query.ToComponentDataArray<LocalTransform>(Allocator.Temp)[0];

        //задать позицию
        transform.position = targetTransform.Position;

        //задать размер
        radius.transform.localScale = new(target.maxDistance, target.maxDistance, target.maxDistance);
    }
}
