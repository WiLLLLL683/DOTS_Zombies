using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Core;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public struct OneTimeSpawner : IComponentData, IEnableableComponent
{
    public Entity prefab;
    public int spawnCount;
    public float spawnRadius;
}
