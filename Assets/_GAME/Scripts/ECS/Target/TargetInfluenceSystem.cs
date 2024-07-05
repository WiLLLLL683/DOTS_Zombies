using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(MainTreadSystemGroup))]
[BurstCompile]
public partial struct TargetInfluenceSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //отключение движения
        foreach (var movementE in SystemAPI
            .Query<EnabledRefRW<MoveToTarget>>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            movementE.ValueRW = false;
        }

        //включение движения только у затронутых целью сущностей 
        foreach (var (target, transform, targetEntity) in SystemAPI
            .Query<RefRW<Target>, LocalTransform>()
            .WithEntityAccess())
        {
            //оверлап сферой вокруг цели
            var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            NativeList<DistanceHit> outHits = new(Allocator.Temp);
            if (!world.OverlapSphere(transform.Position, target.ValueRO.maxDistance, ref outHits, CollisionFilter.Default))
                continue;

            foreach (var hit in outHits)
            {
                if (!SystemAPI.HasComponent<MoveToTarget>(hit.Entity))
                    continue;
                if (!SystemAPI.HasComponent<PhysicsCollider>(hit.Entity))
                    continue;

                //влиять только на указанные в цели слои
                var collider = SystemAPI.GetComponent<PhysicsCollider>(hit.Entity);
                uint belongsTo = collider.Value.Value.GetCollisionFilter().BelongsTo;
                bool isLayerMatch = (belongsTo & target.ValueRO.influenceTo) != 0;
                if (!isLayerMatch)
                    continue;

                var movement = SystemAPI.GetComponentRW<MoveToTarget>(hit.Entity);
                bool wasInfluenced = SystemAPI.IsComponentEnabled<MoveToTarget>(hit.Entity);

                //влияет только ближайщая цель
                if (wasInfluenced && movement.ValueRO.distanceToTarget <= hit.Distance)
                    continue;

                //включение движения
                SystemAPI.SetComponentEnabled<MoveToTarget>(hit.Entity, true);

                //обновление данных для движения
                movement.ValueRW.target = target.ValueRO;
                movement.ValueRW.targetEntity = targetEntity;
                movement.ValueRW.targetPos = transform.Position;
                movement.ValueRW.distanceToTarget = hit.Distance;
            }
        }
    }
}