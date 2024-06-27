using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    [SerializeField] private TMP_Text aliveCountText;
    [SerializeField] private TMP_Text goalPrefab;
    [SerializeField] private Transform goalsParent;
    [SerializeField] private List<TMP_Text> goalTexts = new();

    private EntityManager entityManager;
    private NativeArray<GoalTrigger> goalComponents;
    private NativeArray<Entity> goalEntities;
    private GameState gameState;

    private void Start()
    {
        GetEntities();
        SpawnGoalTexts();
    }

    private void Update()
    {
        GetEntities();

        for (int i = 0; i < goalTexts.Count && i < goalComponents.Length; i++)
        {
            SetGoalData(goalTexts[i], goalEntities[i], goalComponents[i]);
        }

        aliveCountText.text = gameState.zombiesAliveCount.ToString();
    }

    private void OnDestroy()
    {
        
    }

    private void SpawnGoalTexts()
    {
        goalTexts.Clear();
        foreach (var item in goalComponents)
        {
            TMP_Text newText = Instantiate(goalPrefab, goalsParent);
            goalTexts.Add(newText);
        }
    }

    private void GetEntities()
    {
        //получить goals
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var goalQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<GoalTrigger>());
        goalComponents = goalQuery.ToComponentDataArray<GoalTrigger>(AllocatorManager.Temp);
        goalEntities = goalQuery.ToEntityArray(AllocatorManager.Temp);

        //получить gameState
        var gameStateQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<GameState>());
        gameState = gameStateQuery.GetSingleton<GameState>();
    }

    private void SetGoalData(TMP_Text text, Entity entity, GoalTrigger goal)
    {
        string name = entityManager.GetName(entity);
        int count = goal.count;
        int countRequired = goal.countRequired;
        bool isComplete = goal.isComplete;

        text.text = $"{name}: {count}/{countRequired}";

        if (isComplete)
            text.color = Color.green;
        else
            text.color = Color.white;
    }
}