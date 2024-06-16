using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct InputMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ref var inputState = ref SystemAPI.GetSingletonRW<InputState>().ValueRW;

        foreach (var (transform, movement) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<InputMovement>>())
        {
            float3 moveDelta = new(inputState.Horizontal, 0f, inputState.Vertical);
            moveDelta = math.normalizesafe(moveDelta);
            moveDelta = moveDelta * movement.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
            transform.ValueRW.Position += moveDelta;
        }
    }
}
