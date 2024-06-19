using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(TargetMovementSystem))]
[BurstCompile]
public partial struct ZombieAISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZombieAI>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, zombie, groundedEnabled, movementEnabled) in SystemAPI
            .Query<LocalTransform, ZombieAI, EnabledRefRO<Grounded>, EnabledRefRW<TargetMovement>>()
            .WithAll<ZombieAI>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            //проверка на Grounded
            bool isGrounded = groundedEnabled.ValueRO;

            //проверка на нахождение в зоне влияния Target
            Entity targetEntity = SystemAPI.GetSingletonEntity<Target>();
            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);
            float distanceToTarget = math.length(targetTransform.Position - transform.Position);
            bool isInTargetRadius = distanceToTarget < zombie.targetInfluenceDistance;

            //разрешить перемещаться
            movementEnabled.ValueRW = isGrounded & isInTargetRadius;
        }
    }
}