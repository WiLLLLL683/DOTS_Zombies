using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(DeathSystem))]
[UpdateAfter(typeof(GroundedSystem))]
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
        foreach (var (transform, zombie, mass, deadE, groundedE, movementE) in SystemAPI
            .Query<LocalTransform, ZombieAI, RefRW<PhysicsMass>, EnabledRefRO<Dead>, EnabledRefRO<Grounded>, EnabledRefRW<TargetMovement>>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            //разрешить перемещаться только по земле и живым
            movementE.ValueRW = groundedE.ValueRO && !deadE.ValueRO;

            //действия при смерти
            if (deadE.ValueRO)
            {
                mass.ValueRW.InverseInertia = new(1f, 1f, 1f);
            }
        }
    }
}