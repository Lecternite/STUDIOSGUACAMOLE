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
    private float tickRate = 50;
    public float minTimeBetweenTicks;

    private float netTimer = 0;
    private float netTickRate = 30;
    private float netMinTimeBetweenTicks;

    public bool continueSync = true;

    int tickAdjustment = 0;

    bool readySend = false;

    private float multiplier = 1;

    private void Awake()
    {
        instance = this;
        minTimeBetweenTicks = 1f / tickRate;
        netMinTimeBetweenTicks = 1f / netTickRate;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClientOnly)
        {
            CMD_TickStart();
        }
    }

    void Update()
    {
        if (!isServer)
        {
            timer += Time.deltaTime * multiplier;
            multiplier = 1f;//RESET THE MULTIPLIER AFTER ITS ADJUSTment has taken effect

            while (timer >= minTimeBetweenTicks)
            {
                Debug.Log("adjusting tick by " + tickAdjustment.ToString());

                timer -= minTimeBetweenTicks;

                while (tickAdjustment > 0)
                {
                    tick += 1;
                    GameTick?.Invoke(minTimeBetweenTicks);
                    tickAdjustment -= 1;
                }

                if (tickAdjustment < 0)
                {
                    tickAdjustment += 1;
                }
                else
                {
                    tick += 1;
                    GameTick?.Invoke(minTimeBetweenTicks);

                    if (readySend)
                    {
                        requestAnotherSync();
                    }
                }

            }

        }
        else
        {
            timer += Time.deltaTime;

            while (timer >= minTimeBetweenTicks)
            {
                timer -= minTimeBetweenTicks;

                tick += 1;
                GameTick?.Invoke(minTimeBetweenTicks);
            }
        }


        netTimer += Time.deltaTime;

        while (netTimer >= netMinTimeBetweenTicks)
        {
            netTimer -= netMinTimeBetweenTicks;

            SendTime?.Invoke();
        }
    }

    void requestAnotherSync()
    {
        CMD_SyncTick(tick - 4);
        readySend = false;
    }

    [Command(requiresAuthority = false)]
    public void CMD_SyncTick(int clientTick, NetworkConnectionToClient conn = null)
    {
        int adjustment = tick - clientTick;
        RPC_SyncTick(conn, tick, adjustment);
    }

    [TargetRpc]
    public void RPC_SyncTick(NetworkConnection conn, int serverTick, int serverAdjustment)
    {
        if (isClientOnly)
        {
            if(Mathf.Abs(serverAdjustment) > 10)
            {
                tickAdjustment = serverAdjustment;
            }
            else
            {
                multiplier = (float)Math.Tanh(tickAdjustment / 6d) + 1;// SMOOTHLY APPLY AN ADJUSTMENT TO THE LOCAL CLOCK - MAYBE A SIMPLER WAY IS BETTER
            }

            readySend = true;
        }
    }

    [Command(requiresAuthority = false)]
    public void CMD_TickStart(NetworkConnectionToClient conn = null)
    {
        RPC_TickStart(conn, tick);
    }

    [TargetRpc]
    public void RPC_TickStart(NetworkConnection conn, int serverTick)
    {
        tick = serverTick + 5;

        CMD_SyncTick(tick);

        readySend = true;
    }

}
