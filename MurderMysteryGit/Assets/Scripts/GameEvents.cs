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
    }

    public GameState gameState = GameState.waitingForPlayers;

    // EVENTS
    public UnityEvent<GameState> GameStateEntered;
    public UnityEvent<GameState> GameStateExited;

    void handlePlayer(playerScript player)
    {
        GameStateEntered.AddListener(player.handleStateChange);
        playerScript.playerCreated -= handlePlayer;
    }

    private void Awake()
    {
        playerScript.playerCreated += handlePlayer;
    }

    // PUBLIC
    public void EnterGameState(GameState _gameState)
    {
        GameStateExited?.Invoke(gameState);
        gameState = _gameState;
        GameStateEntered?.Invoke(gameState);
    }

    public void setUserName(string name)
    {
        playerUserName = name;
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
    public void toserver_requestNames(NetworkIdentity netID)
    {
        toclient_requestNames(netID);
    }

    [ClientRpc]
    public void toclient_requestNames(NetworkIdentity netID)
    {
        if(!netID.hasAuthority)
        {
            toserver_sendName(NetworkClient.localPlayer, playerUserName);
        }
    }

    private void Update()
    {
        if (Inputter.Instance.playerInput.actions["Ready Up"].WasPerformedThisFrame())
        {
            server_changeReady(!localPlayerReady);
            localPlayerReady = !localPlayerReady;
        }

        if (!isServer)
        {
            return;
        }
        
        if(numReady >= NetworkServer.connections.Count && gameState != GameState.gracePeriod)
        {
            server_invokeStart(GameState.gracePeriod);
        }

    }

    public string getNumPlayers()
    {
        return NetworkServer.connections.Count.ToString();
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

    [Command(requiresAuthority = false)]
    void server_invokeStart(GameState state)
    {
        client_invokeStart(state);
    }

    [ClientRpc]
    void client_invokeStart(GameState state)
    {
        EnterGameState(GameState.gracePeriod);
    }
    
}