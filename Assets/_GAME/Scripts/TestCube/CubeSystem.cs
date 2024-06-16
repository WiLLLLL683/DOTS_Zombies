using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

[BurstCompile]
public partial struct CubeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, cube) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Cube>>())
        {
            float3 moveDelta = cube.ValueRO.moveDirection * cube.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
            localTransform.ValueRW.Position += moveDelta;

            if (cube.ValueRO.moveSpeed > 0)
            {
                cube.ValueRW.moveSpeed -= 1 * SystemAPI.Time.DeltaTime;
            }
            else
            {
                cube.ValueRW.moveSpeed = 0;
            }
        }
    }
}
