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
        var inputState = SystemAPI.GetSingleton<InputState>();

        foreach (var (transform, movement) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<InputMovement>>())
        {
            float3 moveDelta = new(inputState.Movement.x,0f, inputState.Movement.y);
            moveDelta = math.normalizesafe(moveDelta);
            moveDelta = moveDelta * movement.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
            transform.ValueRW.Position += moveDelta;
        }
    }
}
