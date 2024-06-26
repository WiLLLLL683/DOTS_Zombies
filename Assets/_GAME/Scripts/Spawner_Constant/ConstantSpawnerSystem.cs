using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

[UpdateInGroup(typeof(MainTreadSystemGroup))]
[BurstCompile]
public partial struct ConstantSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ConstantSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new(Allocator.Temp);

        foreach(var (spawner, transform) in SystemAPI.Query<RefRW<ConstantSpawner>, RefRO<LocalTransform>>())
        {
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                //������� ����� entity
                Entity entity = buffer.Instantiate(spawner.ValueRO.prefab);

                //������ ��������� entity
                buffer.SetComponent(entity, new LocalTransform
                {
                    Position = transform.ValueRO.Position + spawner.ValueRO.offset,
                    Rotation = quaternion.identity,
                    Scale = 1
                });

                //��� ������ ������
                buffer.AddComponent(entity, new NewSpawn());

                //�������� ������ ������
                spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            }
        }

        buffer.Playback(state.EntityManager);
        buffer.Dispose();
    }
}
