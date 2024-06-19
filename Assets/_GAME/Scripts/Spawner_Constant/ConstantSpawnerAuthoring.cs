using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ConstantSpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 offset;
    public float spawnRate;
}

class SpawnerBaker : Baker<ConstantSpawnerAuthoring>
{
    public override void Bake(ConstantSpawnerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new ConstantSpawner
        {
            prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
            offset = authoring.offset,
            nextSpawnTime = 0f,
            spawnRate = authoring.spawnRate
        }); ;
    }
}
