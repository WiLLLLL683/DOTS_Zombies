using Unity.Entities;
using Unity.Mathematics;

public struct MoveAvoid : IComponentData, IEnableableComponent
{
    public float speed;
    public float maxDistance;
    public float minDistance;
    public uint influenceBy;
}
