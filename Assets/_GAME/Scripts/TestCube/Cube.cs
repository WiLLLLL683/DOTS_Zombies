using Unity.Entities;
using Unity.Mathematics;

public struct Cube : IComponentData
{
    public bool isNew;
    public float3 moveDirection;
    public float moveSpeed;
}
