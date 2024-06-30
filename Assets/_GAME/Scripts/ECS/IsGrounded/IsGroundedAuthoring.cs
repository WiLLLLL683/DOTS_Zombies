using System;
using System.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class IsGroundedAuthoring : MonoBehaviour
{
    public Vector3 offset;
    public float threshold;
    public float maxRayLength;
    public PhysicsCategoryTags groundLayer;
    public PhysicsCategoryTags rayCastLayer;

    class Baker : Baker<IsGroundedAuthoring>
    {
        public override void Bake(IsGroundedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new IsGrounded
            {
                offset = authoring.offset,
                threshold = authoring.threshold,
                maxRayLength = authoring.maxRayLength,
                groundLayer = authoring.groundLayer.Value,
                rayCastLayer = authoring.rayCastLayer.Value
            });

            SetComponentEnabled<IsGrounded>(entity, false);
        }
    }
}
