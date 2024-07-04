using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class MoveOnInputAuthoring : MonoBehaviour
{
    public float moveSpeed;
}

class InputMovementBaker : Baker<MoveOnInputAuthoring>
{
    public override void Bake(MoveOnInputAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new MoveOnInput
        {
            moveSpeed = authoring.moveSpeed
        });
    }
}