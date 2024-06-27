using Unity.Entities;
using Unity.Mathematics;

public struct GoalTrigger : IComponentData
{
    public int countRequired;
    public int count;
    public bool isFull;
    public uint triggerLayer;
}