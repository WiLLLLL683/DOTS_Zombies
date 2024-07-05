using Unity.Entities;
using Unity.Mathematics;

public struct MoveToTarget : IComponentData, IEnableableComponent
{
    public float speed;
    public Target target;
    public Entity targetEntity;
    public float3 targetPos;
    public float distanceToTarget;
}
