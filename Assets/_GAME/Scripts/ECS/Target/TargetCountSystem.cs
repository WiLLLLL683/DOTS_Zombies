using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
public partial struct TargetCountSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var target = SystemAPI.GetSingletonRW<Target>();

        target.ValueRW.count = 0;

        foreach (var movement in SystemAPI
            .Query<RefRO<TargetMovement>>())
        {
            if (!movement.ValueRO.isMoving)
                continue;

            target.ValueRW.count++;
        }
    }
}