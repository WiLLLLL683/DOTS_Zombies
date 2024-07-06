using Unity.Entities;
using Unity.Mathematics;

public struct TargetInfluence : IComponentData, IEnableableComponent
{
    public Target target;
    public Entity targetEntity;
    public float3 targetPos;
    public float distanceToTarget;
}
