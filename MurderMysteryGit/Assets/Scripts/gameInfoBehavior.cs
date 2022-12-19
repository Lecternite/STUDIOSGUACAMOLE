using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public delegate void procedure();

public class gameInfoBehavior : MonoBehaviour
{
    TMP_Text tm;
    public GameEvents gameEvents;

    string imposter = "Innocent";

    playerScript player;

    procedure deal_with_player = () => { };

    private void Awake()
    {
        tm = GetComponent<TMP_Text>();

        DependencyHelper<playerScript>.SpawnEvent += SetPlayer;
    }

    void SetPlayer(playerScript ps, uint id)
    {
        player = ps;

        deal_with_player = () =>
        {
            if (player.imposter)
            {
                imposter = "Imposter";
            }
        };

        DependencyHelper<playerScript>.SpawnEvent -= SetPlayer;
    }

    void Update()
    {
        deal_with_player.Invoke();
        if(Clocky.instance != null && gameEvents != null)
        {
            tm.text = "Tick: " + Clocky.instance.tick.ToString() + " | " + gameEvents.gameState.ToString() + " | " + NetworkServer.connections.Count.ToString() + " | " + gameEvents.numReady.ToString() + " | " + imposter + "\n" + "Avg tick offset from server: " + Clocky.instance.avgTickOffset.ToString("F3") + " | " + DependencyHelper<gunScript>.host_instances.Count;
        }
    }
}
