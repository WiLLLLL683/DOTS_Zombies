using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate;
}

class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new Spawner
        {
            prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
            nextSpawnTime = 0f,
            spawnRate = authoring.spawnRate
        });
    }
}
