using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateBefore(typeof(TargetMovementSystem))]
[BurstCompile]
public partial struct ZombieAISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, groundedEnabled, movementEnabled) in SystemAPI
            .Query<RefRW<LocalTransform>, EnabledRefRW<Grounded>, EnabledRefRW<TargetMovement>>()
            .WithAll<ZombieAI>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            movementEnabled.ValueRW = groundedEnabled.ValueRO;
        }
    }
}