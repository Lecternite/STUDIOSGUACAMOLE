using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityLagTesterScript : NetworkBehaviour
{

    Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;

        Clocky.instance.GameTick += update;
        if (isServer)
        {
            Clocky.instance.SendTime += handleSendTime;
            EntityHistory.Instance.trackedEntities.Add(gameObject);
        }
    }

    void update(float deltaTime)
    {
        if (isServer)
        {
            transform.position = startingPos + new Vector3(Mathf.Cos(Clocky.instance.tick * 0.03f) * 2f, 0, Mathf.Sin(Clocky.instance.tick * 0.03f) * 6f);
        }
    }

    void handleSendTime()
    {
        RPC_SyncPosition(transform.position);
    }

    [ClientRpc]
    void RPC_SyncPosition(Vector3 pos)
    {
        if (!isServer)
        {
            transform.position = pos;
        }
    }

    [ClientRpc]
    public void RPC_Indicate()
    {
        Vector3 c = (Random.insideUnitSphere + Vector3.one) / 2f;
        GetComponent<MeshRenderer>().material.color = new Color(c.x, c.y, c.z);
    }
}
