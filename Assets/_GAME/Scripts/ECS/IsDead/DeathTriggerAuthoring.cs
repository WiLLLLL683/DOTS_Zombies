using Unity.Entities;
using UnityEngine;

class DeathTriggerAuthoring : MonoBehaviour
{
    class Baker : Baker<DeathTriggerAuthoring>
    {
        public override void Bake(DeathTriggerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new DeathTrigger());
        }
    }
}
