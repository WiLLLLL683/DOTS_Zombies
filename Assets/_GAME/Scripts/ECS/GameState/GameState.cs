using Unity.Entities;
using Unity.Mathematics;

public struct GameState : IComponentData
{
    public bool isWin;
    public bool isLose;
    public int zombiesAliveCount;
    public int zombiesAvailableCount;
    public int zombiesRequiredCount;
    public int goalCount;
    public int fullGoalCount;
}