using UnityEngine;
using UnityEngine.Events;
using System;

public class GameMotor : MonoBehaviour
{
    public GameState activeState = new GameStateWaiting();

    public event Action<GameState> OnGameStateEnter;
    public event Action<GameState> OnGameStateLeave;

    private void Start()
    {
        activeState.motor = this;
        activeState.Construct();
        OnGameStateEnter?.Invoke(activeState);
    }

    private void Update()
    {
        activeState.UpdateState();
    }

    public void ChangeState(GameState state)
    {
        activeState.Destruct();
        OnGameStateLeave?.Invoke(state);
        activeState = state;
        activeState.motor = this;
        activeState.Construct();
        OnGameStateEnter?.Invoke(state);
    }
}
