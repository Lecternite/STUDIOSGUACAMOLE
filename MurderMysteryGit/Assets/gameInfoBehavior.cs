using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class gameInfoBehavior : MonoBehaviour
{
    TMP_Text tm;
    public GameEvents gameEvents;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<TMP_Text>();
        //gameEvents = FindObjectOfType<GameEvents>();
    }

    // Update is called once per frame
    void Update()
    {
        tm.text = gameEvents.gameState.ToString() + " | " + gameEvents.getNumPlayers() + " | " + gameEvents.numReady.ToString();
    }
}
