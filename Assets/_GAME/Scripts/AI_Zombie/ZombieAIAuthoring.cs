using Unity.Entities;
using UnityEngine;

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
