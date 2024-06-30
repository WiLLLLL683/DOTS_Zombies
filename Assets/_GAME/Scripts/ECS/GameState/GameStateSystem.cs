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

        //подсчет доступных зомби
        gameState.ValueRW.zombiesAvailableCount = 0;
        foreach (var zombie in SystemAPI
            .Query<ZombieAI>()
            .WithNone<IsDead, IsInGoal>())
        {
            gameState.ValueRW.zombiesAvailableCount++;
        }

        //подсчет заполненных целей
        gameState.ValueRW.goalCount = 0;
        gameState.ValueRW.fullGoalCount = 0;
        gameState.ValueRW.zombiesRequiredCount = 0;

        foreach (var goal in SystemAPI.Query<GoalTrigger>())
        {
            gameState.ValueRW.goalCount++;
            gameState.ValueRW.zombiesRequiredCount  += goal.countRequired - goal.count;

            if (goal.isComplete)
            {
                gameState.ValueRW.fullGoalCount++;
            }
        }

        //проверка проигрыша
        if (gameState.ValueRO.zombiesAvailableCount < gameState.ValueRO.zombiesRequiredCount)
        {
            gameState.ValueRW.isLose = true;
            gameState.ValueRW.isWin = false;
            OnLose?.Invoke();
            return;
        }

        //TODO проверка победы
        if (gameState.ValueRO.fullGoalCount == gameState.ValueRO.goalCount)
        {
            gameState.ValueRW.isLose = false;
            gameState.ValueRW.isWin = true;
            OnWin?.Invoke();
            return;
        }
    }
}