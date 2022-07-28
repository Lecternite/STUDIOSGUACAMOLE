using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityState
{
    public GameObject gameObject;
    public Vector3 position;
    public Vector3 temp;

    public EntityState(GameObject gameObject, Vector3 position)
    {
        this.gameObject = gameObject;
        this.position = position;
    }
}

public class Record
{
    public int thisTick;
    public List<EntityState> entities = new List<EntityState>();
}

