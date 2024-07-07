using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(MyPhysicsGroup))]
[BurstCompile]
public partial struct MoveAvoidSystem : ISystem
{
    private ComponentLookup<Avoid> avoidLookup;
    private ComponentLookup<LocalTransform> transformLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Avoid>();
        state.RequireForUpdate<MoveAvoid>();
        avoidLookup = state.GetComponentLookup<Avoid>(true);
        transformLookup = state.GetComponentLookup<LocalTransform>(true);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        avoidLookup.Update(ref state);
        transformLookup.Update(ref state);

        state.Dependency = new MoveAvoidJob
        {
            world = world,
            avoidLookup = avoidLookup,
            transformLookup = transformLookup
        }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    partial struct MoveAvoidJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld world;
        [ReadOnly] public ComponentLookup<Avoid> avoidLookup;
        [ReadOnly] public ComponentLookup<LocalTransform> transformLookup;

        public void Execute(ref MoveAvoid movement, ref PhysicsVelocity velocity, Entity entity)
        {
            //оверлап сферой вокруг передвигаемого объекта
            var transform = transformLookup.GetRefRO(entity).ValueRO;
            var outHits = new NativeList<DistanceHit>(Allocator.Temp);
            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = movement.influenceBy
            };
            if (!world.OverlapSphere(transform.Position, movement.maxDistance, ref outHits, CollisionFilter.Default))
                return;

            //расчет направления от всех Avoid
            float3 averageDirection = float3.zero;
            float minDistance = 0;
            foreach (var hit in outHits)
            {
                if (!avoidLookup.HasComponent(hit.Entity))
                    continue;
                if (!transformLookup.TryGetComponent(hit.Entity, out LocalTransform avoidTransform))
                    continue;

                float3 direction = avoidTransform.Position - transform.Position;
                averageDirection += math.normalizesafe(direction);
                float distance = math.length(direction);
                minDistance = math.min(minDistance, distance);
            }
            averageDirection = -math.normalizesafe(averageDirection);
            outHits.Dispose();

            //расчет скорости
            float speed = math.lerp(movement.speed, 0, minDistance / movement.maxDistance);
            float3 newVelocity = averageDirection * speed;
            newVelocity.y = velocity.Linear.y;

            //перемещение
            velocity.Linear = newVelocity;
        }
    }
}