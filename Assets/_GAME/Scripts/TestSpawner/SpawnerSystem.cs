using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new(Allocator.Temp, PlaybackPolicy.MultiPlayback);

        foreach(var spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                //создать новую entity
                Entity entity = buffer.Instantiate(spawner.ValueRO.prefab);

                //задать положение entity
                buffer.SetComponent(entity, new LocalTransform
                {
                    Position = spawner.ValueRO.spawnPosition,
                    Rotation = quaternion.identity,
                    Scale = 1
                });

                //обновить таймер спавна
                spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            }
        }

        buffer.Playback(state.EntityManager);
    }
}
