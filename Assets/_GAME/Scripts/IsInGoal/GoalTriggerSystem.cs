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
    private ComponentLookup<PhysicsCollider> colliders;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<IsDead>();
        state.RequireForUpdate<DeathTrigger>();

        goalTriggers = state.GetComponentLookup<GoalTrigger>();
        inGoalTags = state.GetComponentLookup<IsInGoal>();
        colliders = state.GetComponentLookup<PhysicsCollider>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        goalTriggers.Update(ref state);
        inGoalTags.Update(ref state);
        colliders.Update(ref state);

        state.Dependency = new GoalTriggerJob
        {
            goalTriggers = goalTriggers,
            inGoalTags = inGoalTags,
            colliders = colliders
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    partial struct GoalTriggerJob : ITriggerEventsJob
    {
        public ComponentLookup<GoalTrigger> goalTriggers;
        public ComponentLookup<IsInGoal> inGoalTags;
        public ComponentLookup<PhysicsCollider> colliders;

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
            var trigger = goalTriggers.GetRefRW(triggerEntity);
            var collider = colliders.GetRefRW(tagEntity);

            //включить тэг
            inGoalTags.GetEnabledRefRW<IsInGoal>(tagEntity).ValueRW = true;

            //отключить коллизии с триггером
            var filter = collider.ValueRO.Value.Value.GetCollisionFilter();
            filter.CollidesWith ^= trigger.ValueRO.triggerLayer;
            collider.ValueRW.Value.Value.SetCollisionFilter(filter);
            //collider.ValueRW.Value.Value.SetCollisionFilter(CollisionFilter.Zero);

            //добавить в счетчик цели
            trigger.ValueRW.count++;
            trigger.ValueRW.isFull = trigger.ValueRO.count >= trigger.ValueRO.countRequired;
        }
    }
}