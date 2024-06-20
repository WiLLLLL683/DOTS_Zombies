using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
public partial struct ZombieAISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZombieAI>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, zombie, groundedEnabled, movementEnabled) in SystemAPI
            .Query<LocalTransform, ZombieAI, EnabledRefRO<Grounded>, EnabledRefRW<TargetMovement>>()
            .WithAll<ZombieAI>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            //разрешить перемещаться только по земле 
            movementEnabled.ValueRW = groundedEnabled.ValueRO;
        }
    }
}