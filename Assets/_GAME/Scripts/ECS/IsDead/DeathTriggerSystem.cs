using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(JobSystemGroup))]
//[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct DeathTriggerSystem : ISystem
{
    private ComponentLookup<DeathTrigger> deathTriggers;
    private ComponentLookup<IsDead> deadTags;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<IsDead>();
        state.RequireForUpdate<DeathTrigger>();

        deathTriggers = state.GetComponentLookup<DeathTrigger>(true);
        deadTags = state.GetComponentLookup<IsDead>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        deathTriggers.Update(ref state);
        deadTags.Update(ref state);

        var job = new DeathTriggerJob
        {
            deathTriggers = deathTriggers,
            deadTags = deadTags,
        };

        state.Dependency = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    struct DeathTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<DeathTrigger> deathTriggers;
        public ComponentLookup<IsDead> deadTags;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (deathTriggers.HasComponent(entityA) && deadTags.HasComponent(entityB))
            {
                deadTags.GetEnabledRefRW<IsDead>(entityB).ValueRW = true;
            }
            else if (deathTriggers.HasComponent(entityB) && deadTags.HasComponent(entityA))
            {
                deadTags.GetEnabledRefRW<IsDead>(entityA).ValueRW = true;
            }
        }
    }
}