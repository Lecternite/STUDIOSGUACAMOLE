using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityHistory : MonoBehaviour
{
    public static EntityHistory Instance;

    public List<GameObject> trackedEntities = new List<GameObject>();

    const int bufferSize = 50; // number of frames that is stored in history
    public RecordCollection[] historyBuffer = new RecordCollection[bufferSize];

    public static event Action<EntityHistory> EntityHistoryCreated;

    private void Awake()
    {
        Instance = this;
        Clocky.Start += clockyStart;
    }

    void clockyStart()
    {
        Clocky.Start -= clockyStart;

        if (Clocky.instance.isServer)//If on the server, then subscribe to lateGameTicks, otherwise, destroy myself
        {
            Clocky.instance.LateGameTick += update;
            EntityHistoryCreated?.Invoke(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void update()
    {
        int currentIndex = (Clocky.instance.tick + 1) % bufferSize;
        historyBuffer[currentIndex] = new RecordCollection();
        for(int i = 0; i < trackedEntities.Count; i++)
        {
            if (trackedEntities[i] == null)
            {
                trackedEntities.RemoveAt(i);
                i -= 1;
            }
            else
            {
                historyBuffer[currentIndex].entities.Add(new EntityRecord(trackedEntities[i], trackedEntities[i].transform.position));
            }
        }
    }

    public bool RayPast(int tick, Ray ray, float maxDist, out RaycastHit hit, GameObject excludeObject = null) //Currently raypast only calculates nearest tick, no interpolation
    {
        RecordCollection currentRecordCollection = historyBuffer[tick % bufferSize];

        //Set all of the tracked entites to the correct positions and get rid of null entities
        for (int i = 0; i < currentRecordCollection.entities.Count; i++)
        {
            if (currentRecordCollection.entities[i].gameObject == excludeObject)
            {
                continue;
            }
            if(currentRecordCollection.entities[i] == null)
            {
                currentRecordCollection.entities.RemoveAt(i);
                i -= 1;
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
            if (currentRecordCollection.entities[i].gameObject == excludeObject)
            {
                continue;
            }
            currentRecordCollection.entities[i].gameObject.transform.position = currentRecordCollection.entities[i].temp;
        }
        Physics.SyncTransforms();

        hit = rHit;
        return result;
    }
}