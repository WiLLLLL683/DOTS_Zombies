﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct Grounded : IComponentData, IEnableableComponent
{
    public float3 offset;
    public float threshold;
    public float maxRayLength;
    public uint groundLayer;
    public uint rayCastLayer;
}

