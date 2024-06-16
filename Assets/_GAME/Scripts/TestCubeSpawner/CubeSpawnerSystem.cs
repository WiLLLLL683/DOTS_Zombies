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
        EntityCommandBuffer buffer = new(Allocator.Temp, PlaybackPolicy.MultiPlayback);

        foreach(var spawner in SystemAPI.Query<RefRW<CubeSpawner>>())
        {
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                //создать новую entity
                Entity entity = buffer.Instantiate(spawner.ValueRO.prefab);

                //задать направление движения в новом компоненте Cube
                float3 moveDirection = Random.CreateFromIndex((uint)(SystemAPI.Time.ElapsedTime / SystemAPI.Time.DeltaTime)).NextFloat3();
                buffer.AddComponent(entity, new Cube { moveDirection = moveDirection, moveSpeed = 10 });

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
