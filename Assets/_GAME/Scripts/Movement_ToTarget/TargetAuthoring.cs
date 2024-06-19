using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;

public class TargetAuthoring : MonoBehaviour
{
    class Baker : Baker<TargetAuthoring>
    {
        public override void Bake(TargetAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Target>(entity);
        }
    }
}
