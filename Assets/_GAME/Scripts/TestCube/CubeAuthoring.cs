using Unity.Entities;
using UnityEngine;

public class CubeAuthoring : MonoBehaviour
{
    public float moveSpeed;
}

class CubeBaker : Baker<CubeAuthoring>
{
    public override void Bake(CubeAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new Cube
        {
            moveSpeed = authoring.moveSpeed
        });

        AddComponent(entity, new NewSpawn());
    }
}
