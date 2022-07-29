using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityHistory : MonoBehaviour
{
    public static EntityHistory Instance;

    public RecordCollection[] historyBuffer = new RecordCollection[50];

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
        List<GameObject> removeThese = new List<GameObject>();

        historyBuffer[(Clocky.instance.tick + 1) % 50] = new RecordCollection();//The transforms are one frame late
        for(int i = 0; i < trackedEntities.Count; i++)
        {
            if (trackedEntities[i] == null)
            {
                removeThese.Add(trackedEntities[i]);
            }
            else
            {
                historyBuffer[(Clocky.instance.tick + 1) % 50].entities.Add(new EntityRecord(trackedEntities[i], trackedEntities[i].transform.position));
            }
        }

        foreach (GameObject go in removeThese)
        {
            trackedEntities.Remove(go);
        }
    }

    public bool RayPast(int tick, Ray ray, float maxDist, out RaycastHit hit)
    {
        RecordCollection currentRecordCollection = historyBuffer[tick % 50];

        //Set all of the tracked entites to the correct positions and get rid of null entities
        for (int i = 0; i < currentRecordCollection.entities.Count; i++)
        {
            if(currentRecordCollection.entities[i] == null)
            {
                currentRecordCollection.entities.RemoveAt(i);
            }
            else
            {
                currentRecordCollection.entities[i].temp = currentRecordCollection.entities[i].gameObject.transform.position;
                currentRecordCollection.entities[i].gameObject.transform.position = currentRecordCollection.entities[i].position;
            }
        }
        Physics.SyncTransforms();

        //Raycast normally
        RaycastHit rHit;
        bool result = Physics.Raycast(ray, out rHit, 100f);

        //Put them back
        for (int i = 0; i < currentRecordCollection.entities.Count; i++)
        {
            currentRecordCollection.entities[i].gameObject.transform.position = currentRecordCollection.entities[i].temp;
        }
        Physics.SyncTransforms();

        hit = rHit;
        return result;
    }
}


