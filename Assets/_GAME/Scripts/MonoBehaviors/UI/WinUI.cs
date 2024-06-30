using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private GameStateSystem system;

    private void Start()
    {
        system = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameStateSystem>();
        system.OnWin += Show;
    }

    private void OnDestroy()
    {
        system.OnWin -= Show;
        Hide();
    }

    private void Show() => canvas.enabled = true;
    private void Hide() => canvas.enabled = false;
}
