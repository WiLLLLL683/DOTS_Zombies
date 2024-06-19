using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct GroundedSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Grounded>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //foreach (var (transform, grounded) in SystemAPI.Query<LocalTransform, EnabledRefRW<Grounded>>())
        //{

        //}
    }
}

