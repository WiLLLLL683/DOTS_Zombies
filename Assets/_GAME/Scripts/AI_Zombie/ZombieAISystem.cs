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
    private ComponentLookup<TargetMovement> movementLookup;
    private ComponentLookup<Dead> deadLookup;
    private ComponentLookup<Grounded> groundedLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZombieAI>();

        movementLookup = state.GetComponentLookup<TargetMovement>();
        deadLookup = state.GetComponentLookup<Dead>(true);
        groundedLookup = state.GetComponentLookup<Grounded>(true);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        movementLookup.Update(ref state);
        deadLookup.Update(ref state);
        groundedLookup.Update(ref state);

        state.Dependency = new ZombieAIJob
        {
            MovementLookup = movementLookup,
            DeadLookup = deadLookup,
            GroundedLookup = groundedLookup
        }.Schedule(state.Dependency);
    }

    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [WithAll(typeof(ZombieAI), typeof(Grounded), typeof(Dead))]
    [BurstCompile]
    partial struct ZombieAIJob : IJobEntity
    {
        public ComponentLookup<TargetMovement> MovementLookup;
        [ReadOnly] public ComponentLookup<Dead> DeadLookup;
        [ReadOnly] public ComponentLookup<Grounded> GroundedLookup;

        public void Execute(ref PhysicsMass mass, Entity entity)
        {
            bool isGrounded = GroundedLookup.IsComponentEnabled(entity);
            bool isDead = DeadLookup.IsComponentEnabled(entity);

            //разрешить перемещаться только по земле и живым
            MovementLookup.SetComponentEnabled(entity, isGrounded && !isDead);

            //действия при смерти
            if (isDead)
            {
                mass.InverseInertia = new(1f, 1f, 1f);
            }
        }
    }
}