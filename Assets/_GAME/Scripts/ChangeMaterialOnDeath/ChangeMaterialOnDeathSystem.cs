using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

[UpdateInGroup(typeof(MainTreadSystemGroup))]
[RequireMatchingQueriesForUpdate]
public partial class ChangeMaterialOnDeathSystem : SystemBase
{
    private Dictionary<Material, BatchMaterialID> materialMapping;

    protected override void OnStartRunning()
    {
        var hybridRenderer = World.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
        materialMapping = new Dictionary<Material, BatchMaterialID>();

        foreach (var change in SystemAPI
            .Query<ChangeMaterialOnDeath>())
        {
            RegisterMaterial(hybridRenderer, change.aliveMaterial);
            RegisterMaterial(hybridRenderer, change.deadMaterial);
        }
    }

    protected override void OnStopRunning()
    {
        UnregisterMaterials();
    }

    protected override void OnUpdate()
    {
        foreach (var (change, materialMeshInfo, entity) in SystemAPI
            .Query<ChangeMaterialOnDeath, RefRW<MaterialMeshInfo>>()
            .WithAll<Dead>()
            .WithEntityAccess())
        {
            materialMeshInfo.ValueRW.MaterialID = materialMapping[change.deadMaterial];
        }
    }

    private void RegisterMaterial(EntitiesGraphicsSystem hybridRendererSystem, Material material)
    {
        // Only register each mesh once, so we can also unregister each mesh just once
        if (!materialMapping.ContainsKey(material))
            materialMapping[material] = hybridRendererSystem.RegisterMaterial(material);
    }

    private void UnregisterMaterials()
    {
        // Can't call this from OnDestroy(), so we can't do this on teardown
        var hybridRenderer = World.GetExistingSystemManaged<EntitiesGraphicsSystem>();
        if (hybridRenderer == null)
            return;

        foreach (var kv in materialMapping)
            hybridRenderer.UnregisterMaterial(kv.Value);
    }
}