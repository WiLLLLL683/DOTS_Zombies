using UGizmo;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

//[UpdateInGroup(typeof(JobSystemGroup))]
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
        foreach (var (target, transform, entity) in SystemAPI
            .Query<Target, LocalTransform>()
            .WithEntityAccess())
        {
            state.Dependency = new TargetMovementJob
            {
                Target = target,
                TargetEntity = entity,
                TargetTransform = transform
            }.Schedule(state.Dependency);
        }
    }

    [BurstCompile]
    partial struct TargetMovementJob : IJobEntity
    {
        [ReadOnly] public Target Target;
        [ReadOnly] public Entity TargetEntity;
        [ReadOnly] public LocalTransform TargetTransform;

        public void Execute(ref LocalTransform transform, ref PhysicsVelocity velocity, ref MoveToTarget movement, PhysicsCollider collider)
        {
            //влиять только на указанные в цели слои
            uint belongsTo = collider.Value.Value.GetCollisionFilter().BelongsTo;
            bool isInfluensed = (belongsTo & Target.influenceTo) != 0;
            if (!isInfluensed)
                return;

            //расчет расстояния до цели
            float distanceToTarget = math.length(TargetTransform.Position - transform.Position);

            //остановить если цель достигнута
            if (distanceToTarget <= Target.minDistance)
            {
                velocity.Linear = new(0f, velocity.Linear.y, 0f);
                movement.isMoving = true;
                movement.influencedBy = TargetEntity;
                return;
            }

            //не перемещать если вне радиуса влияния цели
            if (distanceToTarget > Target.maxDistance)
            {
                movement.isMoving = false;
                movement.influencedBy = Entity.Null;
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
            movement.isMoving = true;
            movement.influencedBy = TargetEntity;
        }
    }
}
