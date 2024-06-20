using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct Grounded : IComponentData, IEnableableComponent
{
    public float3 offset;
    public float threshold;
    public float maxRayLength;
    public uint groundLayer;
    public uint rayCastLayer;

    //дебаг
    public float3 start;
    public float3 end;
    public bool isHit;
    public float hitDistance;
}

