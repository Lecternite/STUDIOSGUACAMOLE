using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityLagTesterScript : NetworkBehaviour
{

    Vector3 startingPos;

    Interpolator interpolator = new Interpolator();

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
        else
        {
            transform.position = interpolator.interpolate(Clocky.instance.tick);
        }
    }

    void handleSendTime()
    {
        RPC_SyncPosition(transform.position, Clocky.instance.tick);
    }

    [ClientRpc]
    void RPC_SyncPosition(Vector3 pos, int tick)
    {
        if (!isServer)
        {
            //transform.position = pos;
            interpolator.addTarget(tick + Clocky.instance.avgTickOffset + 3, pos);
        }
    }

    [ClientRpc]
    public void RPC_Indicate()
    {
        Vector3 c = (Random.insideUnitSphere + Vector3.one) / 2f;
        GetComponent<MeshRenderer>().material.color = new Color(c.x, c.y, c.z);
    }
}
