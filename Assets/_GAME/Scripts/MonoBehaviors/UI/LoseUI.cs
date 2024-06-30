using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private GameStateSystem system;

    private void Start()
    {
        system = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameStateSystem>();
        system.OnLose += Show;
    }

    private void OnDestroy()
    {
        system.OnLose -= Show;
        Hide();
    }

    private void Show() => canvas.enabled = true;
    private void Hide() => canvas.enabled = false;
}
