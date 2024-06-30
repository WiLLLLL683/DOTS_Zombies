using Unity.Entities;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
public partial class JobSystemGroup : ComponentSystemGroup
{

}
