using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class serverReadyCountScript : MonoBehaviour
{
    TMP_Text tm;
    public GameEvents gameEvents;

    void Start()
    {
        tm = GetComponent<TMP_Text>();
    }

    void LateUpdate()
    {
        tm.text = gameEvents.playerUserName;
    }
}
