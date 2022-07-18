using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class gameInfoBehavior : MonoBehaviour
{
    TMP_Text tm;
    public GameEvents gameEvents;

    string imposter = "Innocent";

    playerScript player;

    private void Awake()
    {
        tm = GetComponent<TMP_Text>();
        playerScript.playerCreated += setPlayer;
    }

    void setPlayer(playerScript _player)
    {
        player = _player;
        playerScript.playerCreated -= setPlayer;
    }

    void Update()
    {
        if(player != null)
        {
            if (player.imposter)
            {
                imposter = "Imposter";
            }
        }

        tm.text = "Tick: " + /*Clocky.instance.tick.ToString() +*/ " | " + gameEvents.gameState.ToString() + " | " + gameEvents.getNumPlayers() + " | " + gameEvents.numReady.ToString() + " | " + imposter;
;
    }
}
