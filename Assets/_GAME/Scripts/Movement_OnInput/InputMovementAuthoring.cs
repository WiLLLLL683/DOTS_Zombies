using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class InputMovementAuthoring : MonoBehaviour
{
    public float moveSpeed;
}

class InputMovementBaker : Baker<InputMovementAuthoring>
{
    public override void Bake(InputMovementAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new InputMovement
        {
            moveSpeed = authoring.moveSpeed
        });
    }
}