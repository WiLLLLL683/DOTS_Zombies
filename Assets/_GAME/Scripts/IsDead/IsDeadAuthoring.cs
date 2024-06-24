using Unity.Entities;
using UnityEngine;

class IsDeadAuthoring : MonoBehaviour
{
    class Baker : Baker<IsDeadAuthoring>
    {
        public override void Bake(IsDeadAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new IsDead());
            SetComponentEnabled<IsDead>(entity, false);
        }
    }
}
