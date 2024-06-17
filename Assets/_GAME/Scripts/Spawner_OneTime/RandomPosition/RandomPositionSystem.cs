using Unity.Burst;
using Unity.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct RandomPositionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RandomPosition>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer buffer = new(Allocator.Temp);

        foreach (var (circle, transform, entity) in SystemAPI.Query<RandomPosition, RefRW<LocalTransform>>().WithEntityAccess())
        {
            Random random = Random.CreateFromIndex(circle.seed * (uint)entity.GetHashCode());
            float2 pointInCircle = random.NextFloat2(-circle.radius, circle.radius);
            transform.ValueRW.Position = circle.center + new float3(pointInCircle.x, 0, pointInCircle.y);
            buffer.RemoveComponent<RandomPosition>(entity);
        }

        buffer.Playback(state.EntityManager);
        buffer.Dispose();
    }
}
