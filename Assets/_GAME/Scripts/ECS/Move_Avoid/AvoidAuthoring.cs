using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

class AvoidAuthoring : MonoBehaviour
{
    class Baker : Baker<AvoidAuthoring>
    {
        public override void Bake(AvoidAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Avoid>(entity);
        }
    }
}
