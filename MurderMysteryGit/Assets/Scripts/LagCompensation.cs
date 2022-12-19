using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class LagCompensation : NetworkBehaviour
{
    public static LagCompensation Instance;

    Interpolator interpolator = new Interpolator();

    [HideInInspector]
    public float lagTesterState;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Clocky.instance.SendTime += sendStateToClient;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClientOnly)
        {
            Clocky.instance.GameTick += ProcessInterpolatedLag;
        }
    }

    void sendStateToClient()
    {
        RPC_SyncState(Clocky.instance.tick * 1f, Clocky.instance.tick);
    }

    [ClientRpc]
    void RPC_SyncState(float state, int serverTick)
    {
        if (!isServer)
        {
            interpolator.addTarget(serverTick + Clocky.instance.avgTickOffset + 4, new Vector3(state, 0, 0));
        }
    }

    void ProcessInterpolatedLag(float deltaTime)
    {
        lagTesterState = interpolator.interpolate(Clocky.instance.tick).x;
    }
}
