using UGizmo;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
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
        foreach (var (transform, velocityRW, movement) in SystemAPI
            .Query<LocalTransform, RefRW<PhysicsVelocity>, RefRW<TargetMovement>>())
        {
            ref var velocity = ref velocityRW.ValueRW;

            //������ ���������� �� ����
            var target = SystemAPI.GetSingleton<Target>();
            var targetEntity = SystemAPI.GetSingletonEntity<Target>();
            var targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);
            float distanceToTarget = math.length(targetTransform.Position - transform.Position);

            //���������� ���� ���� ����������
            if (distanceToTarget <= target.minDistance)
            {
                velocity.Linear = new(0f, velocity.Linear.y, 0f);
                continue;
            }

            //�� ���������� ���� ��� ������� ������� ����
            if (distanceToTarget > target.maxDistance)
            {
                continue;
            }

            //�����
            //UGizmos.DrawLine(targetTransform.Position, transform.Position, Color.red);

            //�����������
            float3 direction = targetTransform.Position - transform.Position;
            direction.y = 0;
            direction = math.normalize(direction);
            float3 newVelocity = direction * movement.ValueRO.speed;
            newVelocity.y = velocity.Linear.y;
            velocity.Linear = newVelocity;
        }
    }
}
