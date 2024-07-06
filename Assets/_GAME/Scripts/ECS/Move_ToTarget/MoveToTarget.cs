using Unity.Entities;
using Unity.Mathematics;

public struct MoveToTarget : IComponentData, IEnableableComponent
{
    public float speed;
}
