using Unity.Burst;
using Unity.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct RandomPosition : IComponentData, IEnableableComponent
{
    public uint seed;
    public float radius;
    public float3 center;
}
