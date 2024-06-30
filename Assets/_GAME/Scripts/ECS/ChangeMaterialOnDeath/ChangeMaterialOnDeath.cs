using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public class ChangeMaterialOnDeath : IComponentData
{
    public Material aliveMaterial;
    public Material deadMaterial;
}