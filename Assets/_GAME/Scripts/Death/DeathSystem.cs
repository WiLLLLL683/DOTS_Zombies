using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct DeathSystem : ISystem
{
    private ComponentLookup<DeathTrigger> deathTriggers;
    private ComponentLookup<Dead> deadTags;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Dead>();
        state.RequireForUpdate<DeathTrigger>();

        deathTriggers = state.GetComponentLookup<DeathTrigger>(true);
        deadTags = state.GetComponentLookup<Dead>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>();//.CollisionWorld;

        deathTriggers.Update(ref state);
        deadTags.Update(ref state);

        //NativeReference<int> NumTriggerEvents = new(0, Allocator.TempJob);

        var job = new DeathTriggerJob
        {
            deathTriggers = deathTriggers,
            deadTags = deadTags,
            //NumTriggerEvents = NumTriggerEvents
        };

        state.Dependency = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        //var jobHandle = job.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        //jobHandle.Complete();

        //foreach (var deathTrigger in SystemAPI.Query<RefRW<DeathTrigger>>())
        //{
        //    deathTrigger.ValueRW.triggerEvents = NumTriggerEvents.Value;
        //}

        //NumTriggerEvents.Dispose();
    }

    [BurstCompile]
    struct DeathTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<DeathTrigger> deathTriggers;
        public ComponentLookup<Dead> deadTags;
        //public NativeReference<int> NumTriggerEvents;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (deathTriggers.HasComponent(entityA) && deadTags.HasComponent(entityB))
            {
                deadTags.GetEnabledRefRW<Dead>(entityB).ValueRW = true;
            }
            else if (deathTriggers.HasComponent(entityB) && deadTags.HasComponent(entityA))
            {
                deadTags.GetEnabledRefRW<Dead>(entityA).ValueRW = true;
            }
        }
    }
}