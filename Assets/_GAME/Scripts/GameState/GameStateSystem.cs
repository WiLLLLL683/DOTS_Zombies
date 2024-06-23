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
        if (!SystemAPI.TryGetSingletonRW<GameState>(out var gameState))
            return;

        //подсчет живых зомби
        gameState.ValueRW.zombiesCount = 0;
        foreach (var zombie in SystemAPI
            .Query<ZombieAI>()
            .WithNone<Dead>())
        {
            gameState.ValueRW.zombiesCount++;
        }

        //проверка проигрыша
        if (gameState.ValueRO.zombiesCount <= 0)
        {
            gameState.ValueRW.isLose = true;
            gameState.ValueRW.isWin = false;
            return;
        }

        //TODO проверка победы
    }
}