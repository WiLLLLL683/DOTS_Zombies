using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(DeathSystem))]
[UpdateBefore(typeof(GroundedSystem))]
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
        state.Dependency = new ZombieAIJob().Schedule(state.Dependency);
    }

    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [WithAll(typeof(ZombieAI))]
    [BurstCompile]
    partial struct ZombieAIJob : IJobEntity
    {
        public void Execute(ref PhysicsMass mass, EnabledRefRW<TargetMovement> movement, EnabledRefRO<Grounded> groundE, EnabledRefRO<Dead> deadE)
        {
            bool isGrounded = groundE.ValueRO;
            bool isDead = deadE.ValueRO;

            //разрешить перемещаться только по земле и живым
            movement.ValueRW = !isDead && isGrounded;

            //действия при смерти
            if (isDead)
            {
                mass.InverseInertia = new(1f, 1f, 1f);
            }
        }
    }
}