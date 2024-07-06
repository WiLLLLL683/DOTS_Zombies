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
        foreach (var influenceE in SystemAPI
            .Query<EnabledRefRW<TargetInfluence>>()
            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            influenceE.ValueRW = false;
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

                var influence = SystemAPI.GetComponentRW<TargetInfluence>(hit.Entity);
                bool wasInfluenced = SystemAPI.IsComponentEnabled<TargetInfluence>(hit.Entity);

                //влияет только ближайщая цель
                if (wasInfluenced && influence.ValueRO.distanceToTarget <= hit.Distance)
                    continue;

                //включение влияния
                SystemAPI.SetComponentEnabled<TargetInfluence>(hit.Entity, true);

                //обновление данных для движения
                influence.ValueRW.target = target.ValueRO;
                influence.ValueRW.targetEntity = targetEntity;
                influence.ValueRW.targetPos = transform.Position;
                influence.ValueRW.distanceToTarget = hit.Distance;
            }
        }
    }
}