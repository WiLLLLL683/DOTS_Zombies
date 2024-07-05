using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(JobSystemGroup))]
[UpdateAfter(typeof(ZombieAISystem))]
[BurstCompile]
public partial struct TargetCountSystem : ISystem
{
    public ComponentLookup<Target> targetLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        targetLookup = state.GetComponentLookup<Target>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //сетап для подсчета затронутых сущностей
        foreach (var target in SystemAPI.Query<RefRW<Target>>())
        {
            target.ValueRW.count = 0;
        }

        targetLookup.Update(ref state);

        state.Dependency = new TargetCountJob
        {
            targetLookup = targetLookup
        }.Schedule(state.Dependency);
    }

    [BurstCompile]
    partial struct TargetCountJob : IJobEntity
    {
        public ComponentLookup<Target> targetLookup;

        public void Execute([EntityIndexInQuery] int index, ref MoveToTarget movement)
        {
            targetLookup.GetRefRW(movement.targetEntity).ValueRW.count++;
        }
    }
}