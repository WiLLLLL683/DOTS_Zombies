using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[BurstCompile]
public partial struct GameStateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.EntityManager.CreateSingleton<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //foreach (var (gameState, localToWorld) in SystemAPI
        //    .Query<RefRW<GameState>, RefRW<LocalToWorld>>())
        //{

        //}


    }
}