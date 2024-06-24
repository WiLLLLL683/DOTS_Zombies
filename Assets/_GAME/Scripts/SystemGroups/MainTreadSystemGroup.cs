using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(JobSystemGroup))]
public partial class MainTreadSystemGroup : ComponentSystemGroup
{

}
