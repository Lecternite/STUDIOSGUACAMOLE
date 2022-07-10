using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSwitcher : MonoBehaviour
{
    public GameMotor motor;
    public GameState gameState = new GameStateDeciding();

    void Update()
    {
        if(Inputter.Instance.playerInput.actions["Create Player"].WasPressedThisFrame())
        {
            motor.ChangeState(gameState);
        }
    }
}
