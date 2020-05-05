using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DragDrop : NetworkBehaviour {
    public GameObject Canvas;
    public GameObject DropZone;
    public PlayerManager PlayerManager;

    private bool isDragging = false;
    private bool isOverDropZone = false;
    private bool isDraggable = true;
    private GameObject dropzone;
    private GameObject startParent;
    private Vector2 startPosition;

    private void Start () {
        Canvas = GameObject.Find ("Main Canvas");
        DropZone = GameObject.Find ("DropZone");
        if (!hasAuthority) {
            isDraggable = false;
        }
    }

    void Update () {
        if (isDragging) {
            transform.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent (Canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        isOverDropZone = true;
        dropzone = collision.gameObject;
    }

    private void OnCollisionExit2D (Collision2D collision) {
        isOverDropZone = false;
        dropzone = null;
    }

    public void StartDrag () {
        if (!isDraggable) return;
        startPosition = transform.position;
        startParent = transform.parent.gameObject;
        isDragging = true;
    }

    public void EndDrag () {
        if (!isDraggable) return;
        isDragging = false;
        if (isOverDropZone) {
            transform.SetParent (dropzone.transform, false);
            isDraggable = false;
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager> ();
            PlayerManager.PlayCard (gameObject);
        } else {
            transform.position = startPosition;
            transform.SetParent (startParent.transform, false);
        }
    }
}