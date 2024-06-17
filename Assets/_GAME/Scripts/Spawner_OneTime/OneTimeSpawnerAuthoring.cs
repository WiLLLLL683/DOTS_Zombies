using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class OneTimeSpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public int spawnCount;
    public float spawnRadius;

    public class Baker : Baker<OneTimeSpawnerAuthoring>
    {
        public override void Bake(OneTimeSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new OneTimeSpawner
            {
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                spawnCount = authoring.spawnCount,
                spawnRadius = authoring.spawnRadius
            });
        }
    }
}