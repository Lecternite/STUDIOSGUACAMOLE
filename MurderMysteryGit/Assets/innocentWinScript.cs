using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class innocentWinScript : MonoBehaviour
{
    TMP_Text tmp;
    public GameEvents gameEvents;
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();

        //gameEvents = FindObjectOfType<GameEvents>();
        gameEvents.GameStateEntered.AddListener(HandleGameEnded);
    }

    void HandleGameEnded(GameEvents.GameState gameState)
    {
        if (gameState == GameEvents.GameState.gameEnding)
        {
            gameObject.SetActive(true);
            tmp.color = new Color(1, 1, 1, 1);
        }
    }
}
