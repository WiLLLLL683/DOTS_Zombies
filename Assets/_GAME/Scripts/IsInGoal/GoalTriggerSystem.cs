using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(JobSystemGroup))]
[BurstCompile]
public partial struct GoalTriggerSystem : ISystem
{
    private ComponentLookup<GoalTrigger> goalTriggers;
    private ComponentLookup<IsInGoal> inGoalTags;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<IsDead>();
        state.RequireForUpdate<DeathTrigger>();

        goalTriggers = state.GetComponentLookup<GoalTrigger>(true);
        inGoalTags = state.GetComponentLookup<IsInGoal>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        goalTriggers.Update(ref state);
        inGoalTags.Update(ref state);

        state.Dependency = new GoalTriggerJob
        {
            goalTriggers = goalTriggers,
            inGoalTags = inGoalTags,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    partial struct GoalTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<GoalTrigger> goalTriggers;
        public ComponentLookup<IsInGoal> inGoalTags;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (goalTriggers.HasComponent(entityA) && inGoalTags.HasComponent(entityB))
            {
                inGoalTags.GetEnabledRefRW<IsInGoal>(entityB).ValueRW = true;
            }
            else if (goalTriggers.HasComponent(entityB) && inGoalTags.HasComponent(entityA))
            {
                inGoalTags.GetEnabledRefRW<IsInGoal>(entityA).ValueRW = true;
            }
        }
    }
}