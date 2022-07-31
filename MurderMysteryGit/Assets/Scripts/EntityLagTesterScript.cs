using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityLagTesterScript : NetworkBehaviour
{

    Vector3 startingPos;

    Interpolator interpolator = new Interpolator();

    private void Awake()
    {
        startingPos = transform.position;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        EntityHistory.Instance.trackedEntities.Add(gameObject);
        Clocky.instance.GameTick += update;
        Clocky.instance.SendTime += handleSendTime;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Clocky.instance.GameTick += update;
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
    void RPC_SyncPosition(Vector3 pos, int serverTick)
    {
        if (!isServer)
        {
            //transform.position = pos;
            interpolator.addTarget(serverTick + Clocky.instance.avgTickOffset + 3, pos);
            Debug.Log(interpolator.positions.Count);
        }
    }

    [ClientRpc]
    public void RPC_Indicate()
    {
        Vector3 c = (Random.insideUnitSphere + Vector3.one) / 2f;
        GetComponent<MeshRenderer>().material.color = new Color(c.x, c.y, c.z);
    }
}
