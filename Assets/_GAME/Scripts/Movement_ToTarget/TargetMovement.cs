﻿using Unity.Entities;

public struct TargetMovement : IComponentData, IEnableableComponent
{
    public float speed;
    public float minDistance;
}
