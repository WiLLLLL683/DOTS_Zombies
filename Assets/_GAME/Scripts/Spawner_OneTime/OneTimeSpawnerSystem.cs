using Unity.Burst;
using Unity.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct OneTimeSpawnerSystem : ISystem
{
    private uint seedOffset;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OneTimeSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new(Allocator.Temp);

        foreach (var (spawner, enabled, transform) in SystemAPI.Query<OneTimeSpawner, EnabledRefRW<OneTimeSpawner>, LocalTransform>())
        {
            enabled.ValueRW = false;
            seedOffset += (uint)spawner.spawnCount;

            NativeArray<Entity> entities = new NativeArray<Entity>(spawner.spawnCount, Allocator.Temp);
            buffer.Instantiate(spawner.prefab, entities);
            buffer.AddComponent(entities, new RandomPosition
            {
                radius = spawner.spawnRadius,
                center = transform.Position,
                seed = seedOffset
            });
        }

        buffer.Playback(state.EntityManager);
        buffer.Dispose();
    }

    //[WithAll(typeof(NewSpawn))]
    //[BurstCompile]
    //partial struct RandomPositionJob : IJobEntity
    //{
    //    public uint SeedOffset;
    //    public float Radius;
    //    public float3 Center;

    //    public void Execute([EntityIndexInQuery] int index, ref LocalTransform transform)
    //    {
    //        var random = Random.CreateFromIndex(SeedOffset + (uint)index);
    //        float2 pointInCircle = random.NextFloat2Direction() * Radius;
    //        transform.Position = Center + new float3(pointInCircle.x, 0, pointInCircle.y);
    //    }
    //}
}
