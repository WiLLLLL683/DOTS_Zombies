using UGizmo;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(JobSystemGroup))]
//[UpdateBefore(typeof(PhysicsSystemGroup))]
public partial struct TargetMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Target>();
        state.RequireForUpdate<TargetMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var target = SystemAPI.GetSingleton<Target>();
        var targetEntity = SystemAPI.GetSingletonEntity<Target>();
        var targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

        state.Dependency = new TargetMovementJob
        {
            Target = target,
            TargetTransform = targetTransform
        }.Schedule(state.Dependency);
    }

    [BurstCompile]
    partial struct TargetMovementJob : IJobEntity
    {
        [ReadOnly] public Target Target;
        [ReadOnly] public LocalTransform TargetTransform;

        public void Execute(ref LocalTransform transform, ref PhysicsVelocity velocity, TargetMovement movement)
        {
            //расчет расстояния до цели
            float distanceToTarget = math.length(TargetTransform.Position - transform.Position);

            //остановить если цель достигнута
            if (distanceToTarget <= Target.minDistance)
            {
                velocity.Linear = new(0f, velocity.Linear.y, 0f);
                return;
            }

            //не перемещать если вне радиуса влияния цели
            if (distanceToTarget > Target.maxDistance)
            {
                return;
            }

            //расчет направления
            float3 direction = TargetTransform.Position - transform.Position;
            direction.y = 0;
            direction = math.normalize(direction);

            //расчет скорости
            float speed = math.lerp(movement.speed, 0, distanceToTarget / Target.maxDistance);
            float3 newVelocity = direction * speed;
            newVelocity.y = velocity.Linear.y;

            //перемещение
            velocity.Linear = newVelocity;
        }
    }
}
