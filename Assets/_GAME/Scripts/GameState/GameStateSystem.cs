using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(MainTreadSystemGroup))]
[BurstCompile]
public partial class GameStateSystem : SystemBase
{
    public event Action OnWin;
    public event Action OnLose;

    [BurstCompile]
    protected override void OnCreate()
    {
        EntityManager.CreateSingleton<GameState>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonRW<GameState>(out var gameState))
            return;

        //подсчет живых зомби
        gameState.ValueRW.zombiesAliveCount = 0;
        foreach (var zombie in SystemAPI
            .Query<ZombieAI>()
            .WithNone<IsDead>())
        {
            gameState.ValueRW.zombiesAliveCount++;
        }

        //проверка проигрыша
        if (gameState.ValueRO.zombiesAliveCount <= 0)
        {
            gameState.ValueRW.isLose = true;
            gameState.ValueRW.isWin = false;
            OnLose?.Invoke();
            return;
        }

        //подсчет зомби внутри цели
        gameState.ValueRW.zombiesInGoalCount = 0;
        foreach (var zombie in SystemAPI
            .Query<ZombieAI>()
            .WithAll<IsInGoal>())
        {
            gameState.ValueRW.zombiesInGoalCount++;
        }

        //TODO проверка победы
        if (gameState.ValueRO.zombiesInGoalCount == gameState.ValueRO.zombiesAliveCount)
        {
            gameState.ValueRW.isLose = false;
            gameState.ValueRW.isWin = true;
            OnWin?.Invoke();
            return;
        }
    }
}