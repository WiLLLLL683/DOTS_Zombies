using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

[BurstCompile]
public partial struct CubeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);

        foreach (var entity in entities)
        {
            if (entityManager.HasComponent<Cube>(entity))
            {
                Cube cube = entityManager.GetComponentData<Cube>(entity);
                LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(entity);

                float3 moveDelta = cube.moveDirection * cube.moveSpeed * SystemAPI.Time.DeltaTime;
                localTransform.Position += moveDelta;
                entityManager.SetComponentData(entity, localTransform);

                if (cube.moveSpeed > 0)
                {
                    cube.moveSpeed -= 1 * SystemAPI.Time.DeltaTime;
                }
                else
                {
                    cube.moveSpeed = 0;
                }
                entityManager.SetComponentData(entity, cube);
            }
        }
    }
}
