using Unity.Entities;
using Unity.Mathematics;

public struct GoalTrigger : IComponentData
{
    public int count;
    public int countRequired;
    public bool isComplete;
    public uint triggerLayer;
}