using System.Collections.Generic;
using UnityEngine;

public class EntityRecord
{
    public GameObject gameObject;
    public Vector3 position;
    public Vector3 temp;

    public EntityRecord(GameObject gameObject, Vector3 position)
    {
        this.gameObject = gameObject;
        this.position = position;
    }
}

public class RecordCollection
{
    public int thisTick;
    public List<EntityRecord> entities = new List<EntityRecord>();
}

