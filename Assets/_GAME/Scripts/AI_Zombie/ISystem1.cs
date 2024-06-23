using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
public partial struct ISystem1 : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, localToWorld) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<LocalToWorld>>())
        {

        }

        new ISystem1Job
        {

        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct ISystem1Job : IJobEntity
    {
        public void Execute([EntityIndexInQuery] int index, ref LocalTransform transform)
        {
        }
    }
}