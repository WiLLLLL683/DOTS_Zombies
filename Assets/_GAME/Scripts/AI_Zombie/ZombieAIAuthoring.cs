using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(GroundedAuthoring))]
[RequireComponent(typeof(TargetMovementAuthoring))]
class ZombieAIAuthoring : MonoBehaviour
{
    public float targetInfluenceDistance;

    class Baker : Baker<ZombieAIAuthoring>
    {
        public override void Bake(ZombieAIAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<ZombieAI>(entity, new()
            {
                targetInfluenceDistance = authoring.targetInfluenceDistance
            });
        }
    }
}
