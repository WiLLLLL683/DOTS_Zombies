using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(JobSystemGroup))]
[UpdateBefore(typeof(DeathTriggerSystem))]
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
        public void Execute(ref PhysicsMass mass, EnabledRefRW<MoveToTarget> movementE, EnabledRefRO<IsGrounded> groundedE, EnabledRefRO<IsDead> deadE, EnabledRefRO<IsInGoal> inGoalE)
        {
            bool isGrounded = groundedE.ValueRO;
            bool isDead = deadE.ValueRO;
            bool isInGoal = inGoalE.ValueRO;

            //условия для перемещения 
            movementE.ValueRW = (!isDead && !isInGoal && isGrounded);

            //действия при смерти
            if (isDead)
            {
                mass.InverseInertia = new(1f, 1f, 1f);
            }
        }
    }
}