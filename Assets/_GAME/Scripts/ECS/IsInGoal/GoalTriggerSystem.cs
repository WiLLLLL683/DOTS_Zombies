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

        goalTriggers = state.GetComponentLookup<GoalTrigger>();
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
        public ComponentLookup<GoalTrigger> goalTriggers;
        public ComponentLookup<IsInGoal> inGoalTags;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (goalTriggers.HasComponent(entityA) && inGoalTags.HasComponent(entityB))
            {
                SetInGoal(ref entityA, ref entityB);
            }
            else if (goalTriggers.HasComponent(entityB) && inGoalTags.HasComponent(entityA))
            {
                SetInGoal(ref entityB, ref entityA);
            }
        }

        private void SetInGoal(ref Entity triggerEntity, ref Entity tagEntity)
        {
            //ничего не делать если уже включен тэг
            var isInGoal = inGoalTags.GetEnabledRefRW<IsInGoal>(tagEntity);
            if (isInGoal.ValueRO)
                return;

            //ничего не делать если цель заполнена
            var trigger = goalTriggers.GetRefRW(triggerEntity);
            if (trigger.ValueRO.isComplete)
                return;

            //включить тэг
            isInGoal.ValueRW = true;

            //добавить в счетчик цели
            trigger.ValueRW.count++;
            trigger.ValueRW.isComplete = trigger.ValueRO.count >= trigger.ValueRO.countRequired;
        }
    }
}