using UGizmo;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct MoveToTargetSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Target>();
        state.RequireForUpdate<MoveToTarget>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new TargetMovementJob().Schedule(state.Dependency);
    }

    [BurstCompile]
    partial struct TargetMovementJob : IJobEntity
    {
        public void Execute(ref LocalTransform transform, ref PhysicsVelocity velocity, ref MoveToTarget movement)
        {
            //остановить если цель достигнута
            if (movement.distanceToTarget <= movement.target.minDistance)
            {
                velocity.Linear = new(0f, velocity.Linear.y, 0f);
                return;
            }

            //расчет направления
            float3 direction = movement.targetPos - transform.Position;
            direction.y = 0;
            direction = math.normalize(direction);

            //расчет скорости
            float speed = math.lerp(movement.speed, 0, movement.distanceToTarget / movement.target.maxDistance);
            float3 newVelocity = direction * speed;
            newVelocity.y = velocity.Linear.y;

            //перемещение
            velocity.Linear = newVelocity;
        }
    }
}
