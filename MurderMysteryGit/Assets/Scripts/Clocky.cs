using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Clocky : NetworkBehaviour
{
    //Singleton instance
    public static Clocky instance;

    //Public actions
    public static event Action Start;
    public event Action<float> GameTick;
    public event Action SendTime;
    public event Action LateGameTick;

    //Timing variables
    [HideInInspector]
    public int tick = 0;
    private float timer = 0;
    private float tickRate = 50;
    public float minTimeBetweenTicks;

    //This is the offset from perspective of the client
    public float avgTickOffset = 0f;

    //Sendrate timing variables
    private float netTimer = 0;
    private float netTickRate = 30;
    private float netMinTimeBetweenTicks;

    //Time adjustment variables
    float multiplier = 1f;

    //This is how many ticks ahead of the server we want to be in order for packets to make it on time
    int intendedOffsetFromServer = 4;

    //Timers etc.
    float resendCoolDown = 0;
    float multiplierTimer = 0f;
    bool readySend = false;

    //The average tick offset from persepective of the server - client uses this directly
    float averageCorrection = 0f;
    int avgCorrectionMagnitude = 0;

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
            CMD_TickStart(tick - intendedOffsetFromServer);
        }

        Start?.Invoke();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        Start?.Invoke();
    }

    void Update()
    {
        //CLIENT ONLY
        if (!isServer)
        {
            multiplierTimer = Mathf.Max(-1f, multiplierTimer - Time.deltaTime);//Limit the multiplier tick adjusment to this timer
            if (multiplierTimer <= 0)
            {
                multiplier = 1f;
            }

            timer += Time.deltaTime * multiplier;//Slightly speed/slow time to catch up with the server's offset

            resendCoolDown = Mathf.Max(-1f, resendCoolDown - Time.deltaTime);//Cool down for sending clock sync messages

            while (timer >= minTimeBetweenTicks)
            {
                timer -= minTimeBetweenTicks;

                tick += 1;
                GameTick?.Invoke(minTimeBetweenTicks);

                if (readySend && (resendCoolDown <= 0f && multiplierTimer <= 0f))
                {
                    requestAnotherSync();
                }
            }
        }
        //SERVER AND HOST
        else
        {
            timer += Time.deltaTime;

            while (timer >= minTimeBetweenTicks)
            {
                timer -= minTimeBetweenTicks;

                tick += 1;
                GameTick?.Invoke(minTimeBetweenTicks);
                LateGameTick?.Invoke();//This is for the entity history on server only

                if(tick % 20 == 0)
                {
                    RPC_ServerTick(tick);
                }
            }
        }

        // This clock tells the program when to send messages - currently the same for server and client
        netTimer += Time.deltaTime;

        while (netTimer >= netMinTimeBetweenTicks)
        {
            netTimer -= netMinTimeBetweenTicks;

            SendTime?.Invoke();
        }
    }

    [ClientRpc]
    void RPC_ServerTick(int serverTick)
    {
        if (isClientOnly)
        {
            int offset = tick - serverTick;

            if (Mathf.Abs(offset - avgTickOffset) > 2)
            {
                avgTickOffset = offset;
            }
            else
            {
                avgTickOffset = Mathf.Lerp(avgTickOffset, offset, 0.01f);
            }
        }
    }

    void requestAnotherSync()
    {
        CMD_SyncTick(tick - intendedOffsetFromServer);
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
        if (!isClientOnly)
        {
            return;
        }

        Debug.Log("synctick");
        if (Math.Abs(serverAdjustment) > 10)
        {
            tick += serverAdjustment;
            Debug.Log("tick is being directly adjusted by: " + serverAdjustment.ToString());
        }
        else
        {
            if (avgCorrectionMagnitude < 20)
            {
                averageCorrection += serverAdjustment;
                avgCorrectionMagnitude += 1;
                Debug.Log("Current average: " + (averageCorrection / avgCorrectionMagnitude).ToString());
            }
            else
            {
                averageCorrection /= avgCorrectionMagnitude;
                multiplier = 1f +  averageCorrection / (11f * tickRate);
                multiplierTimer = 9f;
                Debug.Log("The average was: " + averageCorrection.ToString() + " | multiplier: " + multiplier.ToString());
                avgCorrectionMagnitude = 0;
                averageCorrection = 0f;
            }
        }

        readySend = true;

        resendCoolDown = 0.5f;
    }

    [Command(requiresAuthority = false)]
    public void CMD_TickStart(int clientTick, NetworkConnectionToClient conn = null)
    {
        int adjustment = tick - clientTick;
        RPC_TickStart(conn, tick, adjustment);
    }

    [TargetRpc]
    public void RPC_TickStart(NetworkConnection conn, int serverTick, int adjustment)
    {
        Debug.Log("START offset: " + adjustment.ToString());
        tick += adjustment;

        CMD_SyncTick(tick);

        readySend = true;
    }
}
