using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(JobSystemGroup))]
//[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct GroundedSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<IsGrounded>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

        state.Dependency = new GroundedJob
        {
            world = world
        }.ScheduleParallel(state.Dependency);
    }

    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    partial struct GroundedJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld world;

        public void Execute(ref LocalTransform transform, ref IsGrounded grounded, EnabledRefRW<IsGrounded> enabled)
        {
            //параметры рейкаста
            var input = new RaycastInput
            {
                Start = transform.Position + grounded.offset,
                End = transform.Position + grounded.offset + (-transform.Up() * grounded.maxRayLength),
                Filter = new CollisionFilter()
                {
                    BelongsTo = grounded.rayCastLayer,
                    CollidesWith = grounded.groundLayer,
                    GroupIndex = 0
                }
            };

            //рейкаст
            bool isHit = world.CastRay(input, out RaycastHit hit);
            var hitDistance = grounded.maxRayLength * hit.Fraction;

            //проверка на Grounded
            enabled.ValueRW = isHit && (hitDistance <= grounded.threshold);
        }
    }
}

