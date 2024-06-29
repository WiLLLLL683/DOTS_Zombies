using Unity.Entities;

public struct TargetMovement : IComponentData, IEnableableComponent
{
    public float speed;
    public bool isMoving;
}
