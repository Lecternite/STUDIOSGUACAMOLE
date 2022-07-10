using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateChanger2 : MonoBehaviour
{
    public GameMotor motor;
    public GameState state;


    private void Update()
    {
        if (Time.timeSinceLevelLoad > 12f)
        {
            motor.ChangeState(state);
        }
    }
}
