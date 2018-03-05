using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour {

    public float stopTime = 1.0f;
    public float moveTime = 1.0f;
    public Vector3 movement = new Vector3(0, 1, 0);

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private int status = 0;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + movement;
    }

    void Update()
    {

    }

    void OnGUI()
    {
        GUI.TextField(new Rect(410, 10, 200, 20), "transform.position.y:" + transform.position.y);
        GUI.TextField(new Rect(410, 30, 200, 20), "startPosition.y:" + startPosition.y);
        GUI.TextField(new Rect(410, 50, 200, 20), "targetPosition.y:" + targetPosition.y);
    }
}
