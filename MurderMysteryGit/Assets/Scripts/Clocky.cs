using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Clocky : NetworkBehaviour
{
    public static Clocky instance;

    public event Action<float> GameTick;
    public event Action SendTime;

    [HideInInspector]
    public int tick = 0;
    private float timer = 0;
    public float tickRate;
    private float minTimeBetweenTicks;

    private float netTimer = 0;
    private float netTickRate = 20;
    private float netMinTimeBetweenTicks;

    public bool continueSync = true;


    int tickAdjustment = 0;

    private void Awake()
    {
        instance = this;
        minTimeBetweenTicks = 1f / tickRate;
        netMinTimeBetweenTicks = 1f / netTickRate;
        GameTick += handleGameTick;
        SendTime += sendThings;
    }

    private void OnDestroy()
    {
        GameTick -= handleGameTick;
        SendTime -= sendThings;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClientOnly)
        {
            CMD_TickStart(GetComponent<NetworkIdentity>());
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (tickAdjustment != 0)
        {
            Debug.Log("Tick adjustment necessity detected");
        }

        while (timer >= minTimeBetweenTicks)
        {
            timer -= minTimeBetweenTicks;

            if(tickAdjustment >= 0)
            {
                while (tickAdjustment >= 0)
                {
                    tick += 1;

                    GameTick?.Invoke(minTimeBetweenTicks);

                    tickAdjustment -= 1;
                }
                if (tickAdjustment < 0)
                {
                    tickAdjustment = 0;
                }
            }
            else
            {
                tickAdjustment += 1;
            }
        }


        netTimer += Time.deltaTime;

        while (netTimer >= netMinTimeBetweenTicks)
        {
            netTimer -= netMinTimeBetweenTicks;

            SendTime?.Invoke();
        }
    }

    void handleGameTick(float dt)
    {
        //local game stuff
    }

    void sendThings()
    {
        //send things thru rpcs
    }

    float mixInts(int a, int b, float t)
    {
        return (1 - t) * a + t * b;
    }


    [Command(requiresAuthority = false)]
    public void CMD_SyncTick(NetworkIdentity id, int clientTick)
    {
        int adjustment = tick - clientTick;
        RPC_SyncTick(id, tick, adjustment);
    }

    [ClientRpc]
    public void RPC_SyncTick(NetworkIdentity id, int serverTick, int serverAdjustment)
    {
        if(id.netId == GetComponent<NetworkIdentity>().netId)
        {
            if (isClientOnly)
            {
                int myAdjustment = serverTick - tick;
                int avgAdjustment = (int)mixInts(serverAdjustment, myAdjustment, 0.5f);
                tickAdjustment = avgAdjustment;

                if (continueSync)
                {
                    CMD_SyncTick(id, tick);
                    Debug.Log("Sending again! " + myAdjustment.ToString() + " " + serverAdjustment.ToString() + " " + avgAdjustment.ToString());
                }
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CMD_TickStart(NetworkIdentity id)
    {
        RPC_TickStart(id, tick);
    }

    [ClientRpc]
    public void RPC_TickStart(NetworkIdentity id, int serverTick)
    {
        if(id.netId == GetComponent<NetworkIdentity>().netId)
        {
            tick = serverTick + 5;

            CMD_SyncTick(id, tick);
        }
    }
    
}
