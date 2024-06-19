using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
public partial struct TargetMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Target>();
        state.RequireForUpdate<TargetMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, velocityRW, movement) in SystemAPI.Query<LocalTransform, RefRW<PhysicsVelocity>, TargetMovement>())
        {
            ref var velocity = ref velocityRW.ValueRW;

            Entity targetEntity = SystemAPI.GetSingletonEntity<Target>();
            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

            float3 direction = targetTransform.Position - transform.Position;
            direction.y = 0;
            direction = math.normalize(direction);
            float3 newVelocity = direction * movement.speed;
            newVelocity.y = velocity.Linear.y;
            velocity.Linear = newVelocity;// * SystemAPI.Time.DeltaTime;
        }
    }
}
