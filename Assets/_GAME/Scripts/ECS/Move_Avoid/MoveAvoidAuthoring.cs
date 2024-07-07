using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

class MoveAvoidAuthoring : MonoBehaviour
{
    public float speed;
    public float maxRadius;
    public float minRadius;
    public PhysicsCategoryTags influenceBy;

    class Baker : Baker<MoveAvoidAuthoring>
    {
        public override void Bake(MoveAvoidAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveAvoid
            {
                speed = authoring.speed,
                maxDistance = authoring.maxRadius,
                minDistance = authoring.minRadius,
                influenceBy = authoring.influenceBy.Value
            });
        }
    }
}
