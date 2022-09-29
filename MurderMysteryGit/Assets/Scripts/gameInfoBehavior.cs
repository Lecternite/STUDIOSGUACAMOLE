using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        playerScript.playerCreated -= setPlayer;

        player = _player;
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
        if(Clocky.instance != null && gameEvents != null)
        {
            tm.text = "Tick: " + Clocky.instance.tick.ToString() + " | " + gameEvents.gameState.ToString() + " | " + gameEvents.getNumPlayers() + " | " + gameEvents.numReady.ToString() + " | " + imposter + "\n" + "Avg tick offset from server: " + Clocky.instance.avgTickOffset.ToString("F3");
        }
    }
}
