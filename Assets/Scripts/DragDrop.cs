using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour {
    public GameObject canvas;
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropzone;
    private GameObject startParent;
    private Vector2 startPosition;

    private void Awake () {
        canvas = GameObject.Find ("Main Canvas");
    }

    void Update () {
        if (isDragging) {
            transform.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent (canvas.transform, true);
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
        startPosition = transform.position;
        startParent = transform.parent.gameObject;
        isDragging = true;
    }

    public void EndDrag () {
        isDragging = false;
        if (isOverDropZone) {
            transform.SetParent (dropzone.transform, false);
        } else {
            transform.position = startPosition;
            transform.SetParent (startParent.transform, false);
        }
    }
}