using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour {
    public Transform target;
    public float rotateSpeed = 100f;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Input.GetAxis("Horizontal2") * transform.up * rotateSpeed * Time.deltaTime);
        transform.position = target.position;
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(210, 10, 200, 20), "Horizontal2:" + Input.GetAxis("Horizontal2"));
        GUI.TextField(new Rect(210, 30, 200, 20), "Vertical2:" + Input.GetAxis("Vertical2"));
    }
}
