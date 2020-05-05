using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour {
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;

    List<GameObject> cards = new List<GameObject> ();

    public override void OnStartClient () {
        base.OnStartClient ();
        PlayerArea = GameObject.Find ("PlayerArea");
        EnemyArea = GameObject.Find ("EnemyArea");
        DropZone = GameObject.Find ("DropZone");
    }

    [Server]
    public override void OnStartServer () {
        cards.Add (Card1);
        cards.Add (Card2);
    }

    //commands are used to request action from server
    //commands must be prefaced with cmd
    [Command]
    public void CmdDealCards () {
        for (int i = 0; i < 5; i++) {
            GameObject card = Instantiate (cards[Random.Range (0, cards.Count)], Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn (card, connectionToClient);
            RpcShowCard (card, "Dealt");
        }
    }

    public void PlayCard (GameObject card) {
        CmdPlayCard (card);
    }

    [Command]
    void CmdPlayCard (GameObject card) {
        RpcShowCard (card, "Played");
    }

    //client remote procedure call, opposite of cmd
    //needs rpc tag 
    [ClientRpc]
    void RpcShowCard (GameObject card, string type) {
        if (type == "Dealt") {
            //if you have authority then theyre yours
            if (hasAuthority) {
                card.transform.SetParent (PlayerArea.transform, false);
            } else {
                card.transform.SetParent (EnemyArea.transform, false);
            }
        } else if (type == "Played") {
            card.transform.SetParent (DropZone.transform, false);
        }
    }
}