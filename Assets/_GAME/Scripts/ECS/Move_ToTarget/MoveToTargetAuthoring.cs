using System;
using Unity.Entities;
using UnityEngine;

public class MoveToTargetAuthoring : MonoBehaviour
{
    public float speed;

    class Baker : Baker<MoveToTargetAuthoring>
    {
        public override void Bake(MoveToTargetAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<MoveToTarget>(entity, new()
            {
                speed = authoring.speed,
            });
        }
    }
}
