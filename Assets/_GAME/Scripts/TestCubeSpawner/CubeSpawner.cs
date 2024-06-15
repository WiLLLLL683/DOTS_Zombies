using Unity.Entities;
using Unity.Mathematics;

public struct CubeSpawner : IComponentData
{
    public Entity prefab;
    public float spawnRate;
    public float3 spawnPosition;
    public float nextSpawnTime;
}