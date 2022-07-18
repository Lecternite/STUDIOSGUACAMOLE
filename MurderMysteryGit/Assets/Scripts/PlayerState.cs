using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerState
{
    public Vector3 position;
    public Vector3 velocity;

    public PlayerState(Vector3 _position, Vector3 _velocity)
    {
        position = _position;
        velocity = _velocity;
    }
}

public struct InputSnap
{
    public Vector2 moveVec;
    public InputSnap(Vector2 _moveVec)
    {
        moveVec = _moveVec;
    }
}