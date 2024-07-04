using Unity.Entities;

public struct MoveToTarget : IComponentData, IEnableableComponent
{
    public float speed;
    public bool isMoving;
    public Entity influencedBy;
}
