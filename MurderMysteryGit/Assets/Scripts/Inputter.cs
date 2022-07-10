using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputter : MonoBehaviour
{
    public static Inputter Instance;
    public PlayerInput playerInput;
    private void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }
}
