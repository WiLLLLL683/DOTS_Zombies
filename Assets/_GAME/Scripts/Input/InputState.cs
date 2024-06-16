using Unity.Entities;
using Unity.Mathematics;

public struct InputState : IComponentData
{
    public float Horizontal;
    public float Vertical;
    public float MouseX;
    public float MouseY;
}