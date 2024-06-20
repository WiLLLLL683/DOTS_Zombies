using System;
using Unity.Entities;
using UnityEngine;

public class TargetMovementAuthoring : MonoBehaviour
{
    public float speed;

    class Baker : Baker<TargetMovementAuthoring>
    {
        public override void Bake(TargetMovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<TargetMovement>(entity, new()
            {
                speed = authoring.speed,
            });
        }
    }
}
