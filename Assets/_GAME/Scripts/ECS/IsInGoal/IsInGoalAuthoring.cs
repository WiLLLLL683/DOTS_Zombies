using Unity.Entities;
using UnityEngine;

class IsInGoalAuthoring : MonoBehaviour
{


    class Baker : Baker<IsInGoalAuthoring>
    {
        public override void Bake(IsInGoalAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new IsInGoal());
            SetComponentEnabled<IsInGoal>(entity, false);
        }
    }
}
