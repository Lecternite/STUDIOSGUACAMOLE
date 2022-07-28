using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityHistory : MonoBehaviour
{
    public static EntityHistory Instance;

    public Record[] historyBuffer = new Record[50];

    public List<GameObject> trackedEntities = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Clocky.Start += thing;
    }

    void thing()
    {
        Clocky.instance.LateGameTick += update;
        Clocky.Start -= thing;
    }

    void update()
    {
        historyBuffer[(Clocky.instance.tick + 1) % 50] = new Record();
        foreach (GameObject entity in trackedEntities)
        {
            if(entity == null)
            {
                trackedEntities.Remove(entity);
            }
            else
            {
                historyBuffer[(Clocky.instance.tick + 1) % 50].entities.Add(new EntityState(entity, entity.transform.position));
            }
        }
    }

    public bool RayPast(int tick, Ray ray, float maxDist, out RaycastHit hit)
    {
        Record currentRecord = historyBuffer[tick % 50];

        //Set all of the tracked entites to the correct positions
        for (int i = 0; i < currentRecord.entities.Count; i++)
        {
            currentRecord.entities[i].temp = currentRecord.entities[i].gameObject.transform.position;
            currentRecord.entities[i].gameObject.transform.position = currentRecord.entities[i].position;
        }
        Physics.SyncTransforms();

        //Raycast normally
        RaycastHit rHit;
        bool result = Physics.Raycast(ray, out rHit, 100f);

        //Put them back
        for (int i = 0; i < currentRecord.entities.Count; i++)
        {
            currentRecord.entities[i].gameObject.transform.position = currentRecord.entities[i].temp;
        }
        Physics.SyncTransforms();

        hit = rHit;
        return result;
    }
}


