using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
[UpdateBefore(typeof(JobSystemGroup))]
public partial class MainTreadSystemGroup : ComponentSystemGroup
{

}
