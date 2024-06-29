using Unity.Entities;
using Unity.Mathematics;

public struct Target : IComponentData
{
    public float minDistance;
    public float maxDistance;
}