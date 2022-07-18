using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerState
{
    public Vector3 position;
    public Vector3 velocity;
    public int tick;

    public PlayerState(Vector3 _position, Vector3 _velocity, int _tick = 0)
    {
        position = _position;
        velocity = _velocity;
        tick = _tick;
    }
}

public struct InputSnap
{
    public Vector2 moveVec;
    public int tick;
    public InputSnap(Vector2 _moveVec, int _tick = 0)
    {
        moveVec = _moveVec;
        tick = _tick;
    }
}