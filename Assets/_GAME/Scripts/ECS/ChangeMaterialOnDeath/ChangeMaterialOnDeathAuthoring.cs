using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ChangeMaterialOnDeathAuthoring : MonoBehaviour
{
    public Material aliveMaterial;
    public Material deadMaterial;

    class Baker : Baker<ChangeMaterialOnDeathAuthoring>
    {
        public override void Bake(ChangeMaterialOnDeathAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponentObject(entity, new ChangeMaterialOnDeath
            {
                aliveMaterial = authoring.aliveMaterial,
                deadMaterial = authoring.deadMaterial
            });
        }
    }
}
