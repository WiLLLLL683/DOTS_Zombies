using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct CubeInitSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new(Allocator.Temp);

        foreach (var (newSpawn, cube, entity) in SystemAPI.Query<RefRW<NewSpawn>, RefRW<Cube>>().WithEntityAccess())
        {
            cube.ValueRW.moveDirection = Random.CreateFromIndex((uint)(SystemAPI.Time.ElapsedTime / SystemAPI.Time.DeltaTime)).NextFloat3();
            buffer.RemoveComponent<NewSpawn>(entity);
        }

        buffer.Playback(state.EntityManager);
    }
}
