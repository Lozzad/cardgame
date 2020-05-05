using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DrawCards : NetworkBehaviour {
    public PlayerManager playerManager;

    void Start () { }

    public void OnClick () {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager> ();
        playerManager.CmdDealCards ();
    }
}