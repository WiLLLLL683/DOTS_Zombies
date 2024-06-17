using Unity.Entities;
using Unity.Mathematics;

public struct InputState : IComponentData
{
    public float2 Movement;
    public bool Run;
    public bool Jump;
    public bool Attack;
    public float2 PointerPosition;
    public bool PointerClick;
}