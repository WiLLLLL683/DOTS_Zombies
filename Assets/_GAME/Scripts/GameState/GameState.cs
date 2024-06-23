using Unity.Entities;
using Unity.Mathematics;

public struct GameState : IComponentData
{
    public bool isWin;
    public bool isLose;
}