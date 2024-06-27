using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

class GoalTriggerAuthoring : MonoBehaviour
{
    public int countRequired;
    public PhysicsCategoryTags triggerLayer;

    class Baker : Baker<GoalTriggerAuthoring>
    {
        public override void Bake(GoalTriggerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GoalTrigger()
            {
                countRequired = authoring.countRequired,
                triggerLayer = authoring.triggerLayer.Value
            });
        }
    }
}
