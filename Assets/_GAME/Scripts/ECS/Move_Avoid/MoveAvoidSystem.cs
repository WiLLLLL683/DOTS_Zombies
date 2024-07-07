using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct MoveAvoidSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Avoid>();
        state.RequireForUpdate<MoveAvoid>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (movement, transform, velocity) in SystemAPI
            .Query<MoveAvoid, LocalTransform, RefRW<PhysicsVelocity>>())
        {
            //оверлап сферой вокруг передвигаемого объекта
            var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            var outHits = new NativeList<DistanceHit>(Allocator.Temp);
            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = movement.influenceBy
            };
            if (!world.OverlapSphere(transform.Position, movement.maxDistance, ref outHits, CollisionFilter.Default))
                continue;

            //расчет направления от всех Avoid
            float3 averageDirection = float3.zero;
            float minDistance = 0;
            foreach (var hit in outHits)
            {
                if (!SystemAPI.HasComponent<Avoid>(hit.Entity))
                    continue;
                if (!SystemAPI.HasComponent<LocalTransform>(hit.Entity))
                    continue;

                var avoid = SystemAPI.GetComponent<Avoid>(hit.Entity);
                var avoidTransform = SystemAPI.GetComponent<LocalTransform>(hit.Entity);

                float3 direction = avoidTransform.Position - transform.Position;
                averageDirection += math.normalizesafe(direction);
                float distance = math.length(direction);
                minDistance = math.min(minDistance, distance);
            }
            averageDirection = -math.normalizesafe(averageDirection);

            //расчет скорости
            float speed = math.lerp(movement.speed, 0, minDistance / movement.maxDistance);
            float3 newVelocity = averageDirection * speed;
            newVelocity.y = velocity.ValueRO.Linear.y;

            //перемещение
            velocity.ValueRW.Linear = newVelocity;
        }

            //new MoveAvoidSystemJob
            //{

            //}.ScheduleParallel();
        }

    [BurstCompile]
    partial struct MoveAvoidSystemJob : IJobEntity
    {
        public void Execute([EntityIndexInQuery] int index, ref LocalTransform transform)
        {
        }
    }
}