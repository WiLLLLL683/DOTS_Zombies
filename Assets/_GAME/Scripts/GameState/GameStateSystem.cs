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
        gameState.ValueRW.zombiesCount = 0;
        foreach (var zombie in SystemAPI
            .Query<ZombieAI>()
            .WithNone<IsDead>())
        {
            gameState.ValueRW.zombiesCount++;
        }

        //проверка проигрыша
        if (gameState.ValueRO.zombiesCount <= 0)
        {
            gameState.ValueRW.isLose = true;
            gameState.ValueRW.isWin = false;
            OnLose?.Invoke();
            return;
        }

        //TODO проверка победы
    }
}