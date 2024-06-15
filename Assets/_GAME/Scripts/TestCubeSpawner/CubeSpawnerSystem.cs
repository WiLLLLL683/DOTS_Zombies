using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

[BurstCompile]
public partial struct CubeSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<CubeSpawner>(out Entity spawnerEntity))
            return;

        RefRW<CubeSpawner> spawner = SystemAPI.GetComponentRW<CubeSpawner>(spawnerEntity);
        EntityCommandBuffer buffer = new(Allocator.Temp);

        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            Entity newEntity = buffer.Instantiate(spawner.ValueRO.prefab);
            float3 moveDirection = Random.CreateFromIndex((uint)(SystemAPI.Time.ElapsedTime / SystemAPI.Time.DeltaTime)).NextFloat3();
            buffer.AddComponent(newEntity, new Cube { moveDirection = moveDirection, moveSpeed = 10 });

            spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            buffer.Playback(state.EntityManager);
        }
    }
}
