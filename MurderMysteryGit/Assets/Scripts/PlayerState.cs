using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerState
{
    public Vector3 position;
    public Vector3 velocity;
    public int tick;
    public Vector3 gNormal;
    public bool grounded;

    public PlayerState(Vector3 _position, Vector3 _velocity, int _tick = 0, Vector3 _gNormal = new Vector3(), bool _grounded = false)
    {
        position = _position;
        velocity = _velocity;
        tick = _tick;
        gNormal = _gNormal;
        grounded = _grounded;
    }
}

public struct InputSnap
{
    public Vector2 moveVec;
    public int tick;
    public Vector3 camTransform;
    public bool jumped;

    public InputSnap(Vector2 _moveVec, int _tick = 0, Vector3 _camTransform = new Vector3(), bool _jumped = false)
    {
        moveVec = _moveVec;
        tick = _tick;
        camTransform = _camTransform;
        jumped = _jumped;
    }
}