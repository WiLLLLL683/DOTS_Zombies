using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(MainTreadSystemGroup))]
public partial class InputSystem : SystemBase
{
    private Controls controls;

    protected override void OnCreate()
    {
        EntityManager.CreateSingleton<InputState>();
        controls = new();
        controls.Enable();
    }

    protected override void OnUpdate()
    {
        ref var gameState = ref SystemAPI.GetSingletonRW<GameState>().ValueRW;
        ref var inputState = ref SystemAPI.GetSingletonRW<InputState>().ValueRW;

        if (gameState.isLose || gameState.isWin)
        {
            inputState.Movement = Vector2.zero;
            inputState.Run = false;
            inputState.Jump = false;
            inputState.Attack = false;
            inputState.PointerPosition = controls.Gameplay.PointerPosition.ReadValue<Vector2>();
            inputState.PointerClick = controls.Gameplay.PointerClick.IsPressed();
        }
        else
        {
            inputState.Movement = controls.Gameplay.Movement.ReadValue<Vector2>();
            inputState.Run = controls.Gameplay.Run.IsPressed();
            inputState.Jump = controls.Gameplay.Jump.IsPressed();
            inputState.Attack = controls.Gameplay.Attack.IsPressed();
            inputState.PointerPosition = controls.Gameplay.PointerPosition.ReadValue<Vector2>();
            inputState.PointerClick = controls.Gameplay.PointerClick.IsPressed();
        }
    }
}
