using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;

public class TargetAuthoring : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;

    class Baker : Baker<TargetAuthoring>
    {
        public override void Bake(TargetAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Target>(entity, new()
            {
                minDistance = authoring.minDistance,
                maxDistance = authoring.maxDistance
            });
        }
    }
}
