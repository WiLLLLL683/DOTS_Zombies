using Unity.Entities;
using Unity.Mathematics;

public struct Spawner : IComponentData
{
    public Entity prefab;
    public float3 offset;
    public float spawnRate;
    public float nextSpawnTime;
}