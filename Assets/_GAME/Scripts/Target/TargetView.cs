using System;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer radius;

    private void Update()
    {
        //получить Target
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var query = entityManager.CreateEntityQuery(typeof(Target));

        if (!query.TryGetSingleton(out Target target))
            return;

        query.TryGetSingletonEntity<Target>(out Entity targetEntity);
        var targetTransform = entityManager.GetComponentData<LocalTransform>(targetEntity);

        //задать позицию
        transform.position = targetTransform.Position;

        //задать размер
        radius.transform.localScale = new(target.maxDistance, target.maxDistance, target.maxDistance);
    }
}
