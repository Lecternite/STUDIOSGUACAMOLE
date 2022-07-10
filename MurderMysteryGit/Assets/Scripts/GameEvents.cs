using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class GameEvents : NetworkBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    public string playerUserName = "unknown";


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
}