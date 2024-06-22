using UGizmo;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct GroundedSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Grounded>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

        foreach (var (transform, enabled, groundedRW) in
            SystemAPI.Query<LocalTransform, EnabledRefRW<Grounded>, RefRW<Grounded>>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            ref var grounded = ref groundedRW.ValueRW;

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

            //дебаг
            //if (isHit)
            //    UGizmos.DrawLine(raycastInput.Start, raycastInput.End, UnityEngine.Color.green);
            //else
            //    UGizmos.DrawLine(raycastInput.Start, raycastInput.End, UnityEngine.Color.grey);

            //проверка на Grounded
            enabled.ValueRW = isHit && (hitDistance <= grounded.threshold);
        }
    }
}

