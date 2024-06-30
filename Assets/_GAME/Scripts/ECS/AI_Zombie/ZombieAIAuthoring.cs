using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(IsGroundedAuthoring))]
[RequireComponent(typeof(TargetMovementAuthoring))]
class ZombieAIAuthoring : MonoBehaviour
{
    class Baker : Baker<ZombieAIAuthoring>
    {
        public override void Bake(ZombieAIAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<ZombieAI>(entity);
        }
    }
}
