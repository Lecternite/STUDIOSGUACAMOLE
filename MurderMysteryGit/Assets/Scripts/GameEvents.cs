using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class GameEvents : NetworkBehaviour
{
    public string playerUserName = "unknown";

    public bool localPlayerReady = false;

    [SyncVar]
    public int numReady = 0;

    public enum GameState
    {
        waitingForPlayers,
        gracePeriod,
        murderMystery,
        gameEnding,
    }

    public GameState gameState = GameState.waitingForPlayers;

    public UnityEvent<GameState> GameStateEntered;
    public UnityEvent<GameState> GameStateExited;

    #region RPC

    [Command(requiresAuthority = false)]
    public void server_EnterGameState(GameState _gameState)
    {
        client_EnterGameState(_gameState);
    }

    [ClientRpc]
    public void client_EnterGameState(GameState _gameState)
    {
        GameStateExited?.Invoke(gameState);
        gameState = _gameState;
        GameStateEntered?.Invoke(gameState);
    }



    [Command(requiresAuthority = false)]
    public void toserver_sendName(NetworkIdentity netID, string name)
    {
        toclients_sendName(netID, name);
    }

    [ClientRpc]
    public void toclients_sendName(NetworkIdentity netID, string name)
    {
        if (!netID.hasAuthority)
        {
            netID.gameObject.GetComponent<playerScript>().nameTag.GetComponent<TMPro.TMP_Text>().text = name;
        }
    }

    [Command(requiresAuthority = false)]
    public void server_requestNames(NetworkIdentity netID)
    {
        client_requestNames(netID);
    }

    [ClientRpc]
    public void client_requestNames(NetworkIdentity netID)
    {
        if(!netID.hasAuthority)
        {
            toserver_sendName(NetworkClient.localPlayer, playerUserName);
        }
    }

    [Command(requiresAuthority = false)]
    void server_invokeStart(GameState state)
    {
        client_invokeStart(state);
    }

    [ClientRpc]
    void client_invokeStart(GameState state)
    {
        client_EnterGameState(GameState.gracePeriod);
    }

    [Command(requiresAuthority = false)]
    public void server_changeReady(bool ready)
    {
        if (ready)
        {
            numReady += 1;
        }
        else
        {
            numReady -= 1;
        }
    }

    #endregion

    private void Start()
    {
        GameStateEntered?.Invoke(gameState);
    }

    private void Update()
    {
        //CLIENT
        if (isClient)
        {
            if (Inputter.Instance.playerInput.actions["Ready Up"].WasPerformedThisFrame())
            {
                server_changeReady(!localPlayerReady);
                localPlayerReady = !localPlayerReady;
            }
        }

        //SERVER
        if (isServer)
        {

            if (numReady >= NetworkServer.connections.Count && gameState == GameState.waitingForPlayers)
            {
                server_invokeStart(GameState.gracePeriod);
            }

            switch (gameState)
            {
                case GameState.gracePeriod:
                    gracePeriodUpdate();
                    break;
            }
        }
    }

    private void gracePeriodUpdate()
    {
        int num = UnityEngine.Random.Range(0, NetworkServer.connections.Count);
        int iter = 0;

        foreach(var client in NetworkServer.connections)
        {
            if(iter == num)
            {
                client.Value.identity.gameObject.GetComponent<playerScript>().setImposter(true);
            }
            iter += 1;
        }

        client_EnterGameState(GameState.murderMystery);
    }

    public void setUserName(string name)
    {
        playerUserName = name;
    }

    public string getNumPlayers()
    {
        return NetworkServer.connections.Count.ToString();
    }

}