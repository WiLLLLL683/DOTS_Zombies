using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(MainTreadSystemGroup))]
[BurstCompile]
public partial struct TargetCountSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (target, targetEntity) in SystemAPI
            .Query<RefRW<Target>>()
            .WithEntityAccess())
        {
            target.ValueRW.count = 0;

            foreach (var movement in SystemAPI.Query<MoveToTarget>())
            {
                if (!movement.isMoving && movement.influencedBy != targetEntity)
                    continue;

                target.ValueRW.count++;
            }
        }
    }
}