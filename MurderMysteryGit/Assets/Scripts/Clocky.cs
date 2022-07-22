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

    private float multiplier = 1f;

    int tickWrongness = 0;

    int intendedOffsetFromServer = 4;

    float resendTimer = 0;

    float multiplierTimer = 0f;

    float averageCorrection = 0f;
    int averageCount = 0;

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
    }

    void Update()
    {
        if (!isServer)
        {
            multiplierTimer = Mathf.Max(-0.0001f, multiplierTimer - Time.deltaTime);
            if (multiplierTimer <= 0)
            {
                multiplier = 1f;
            }

            timer += Time.deltaTime * multiplier;

           // multiplier = 1f;//RESET THE MULTIPLIER AFTER ITS ADJUSTment has taken effect

            resendTimer = Mathf.Max(-0.001f, resendTimer - Time.deltaTime);//Cool down for sending clock sync messages


            while (timer >= minTimeBetweenTicks)
            {
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

                    if (readySend && ((resendTimer <= 0f && multiplierTimer <= 0f) || tickWrongness > 9))
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

        // This clock tells the program when to send messages
        netTimer += Time.deltaTime;

        while (netTimer >= netMinTimeBetweenTicks)
        {
            netTimer -= netMinTimeBetweenTicks;

            SendTime?.Invoke();
        }
    }

    void requestAnotherSync()
    {
        CMD_SyncTick(tick - intendedOffsetFromServer);// the minus four keeps the client tick about 4 ticks ahead of the server tick
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
            Debug.Log("synctick");
            if (Math.Abs(serverAdjustment) > 10)
            {
                tick += serverAdjustment;
                Debug.Log("tick is being directly adjusted by: " + serverAdjustment.ToString());
            }
            else
            {
                if (averageCount < 15)
                {
                    averageCorrection += serverAdjustment;
                    averageCount += 1;
                    Debug.Log("Current average: " + (averageCorrection / averageCount).ToString());
                }
                else
                {
                    averageCorrection /= averageCount;
                    multiplier = (averageCorrection / (8f * tickRate) + 1f);
                    multiplierTimer = 7f;
                    Debug.Log("The average was: " + averageCorrection.ToString() + " | multiplier: " + multiplier.ToString());
                    averageCount = 0;
                    averageCorrection = 0f;
                }
            }

            readySend = true;

            resendTimer = 0.5f;
        }
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
        //tick = serverTick + 5;
        Debug.Log("START offset: " + adjustment.ToString());
        tick += adjustment;

        CMD_SyncTick(tick);

        readySend = true;
    }
}
