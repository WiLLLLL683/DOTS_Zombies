using Unity.Entities;
using Unity.Mathematics;

public struct Spawner : IComponentData
{
    public Entity prefab;
    public float spawnRate;
    public float nextSpawnTime;
}