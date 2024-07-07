using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
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
            .Query<Target, LocalTransform>()
            .WithEntityAccess())
        {
            //оверлап сферой вокруг цели
            var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            var outHits = new NativeList<DistanceHit>(Allocator.Temp);
            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = target.influenceTo
            };
            if (!world.OverlapSphere(transform.Position, target.maxDistance, ref outHits, filter))
                continue;

            foreach (var hit in outHits)
            {
                if (!SystemAPI.HasComponent<TargetInfluence>(hit.Entity))
                    continue;

                var influence = SystemAPI.GetComponentRW<TargetInfluence>(hit.Entity);
                bool wasInfluenced = SystemAPI.IsComponentEnabled<TargetInfluence>(hit.Entity);

                //влияет только ближайщая цель
                if (wasInfluenced && influence.ValueRO.distanceToTarget <= hit.Distance)
                    continue;

                //включение влияния
                SystemAPI.SetComponentEnabled<TargetInfluence>(hit.Entity, true);

                //обновление данных для движения
                influence.ValueRW.target = target;
                influence.ValueRW.targetEntity = targetEntity;
                influence.ValueRW.targetPos = transform.Position;
                influence.ValueRW.distanceToTarget = hit.Distance;
            }
        }
    }
}