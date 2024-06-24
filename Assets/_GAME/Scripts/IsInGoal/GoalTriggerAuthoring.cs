using Unity.Entities;
using UnityEngine;

class GoalTriggerAuthoring : MonoBehaviour
{


    class Baker : Baker<GoalTriggerAuthoring>
    {
        public override void Bake(GoalTriggerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GoalTrigger());
        }
    }
}
