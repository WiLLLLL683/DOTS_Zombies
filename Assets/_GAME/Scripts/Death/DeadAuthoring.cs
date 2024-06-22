using Unity.Entities;
using UnityEngine;

class DeadAuthoring : MonoBehaviour
{
    class Baker : Baker<DeadAuthoring>
    {
        public override void Bake(DeadAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Dead());
            SetComponentEnabled<Dead>(entity, false);
        }
    }
}
