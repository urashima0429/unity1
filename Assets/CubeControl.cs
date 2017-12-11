using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour {

    public float stopTime = 20.0f;
    public Vector3 movement = new Vector3(0, 10, 0);

    private Vector3 startPosition;
    private Vector3 targetPosiotion;
    private int status = 0; //startstop:0, go:1, targetstop:2, return:3
    private Vector3 deltaMove;

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        deltaMove = movement * 0.1f;
        StartCoroutine(Move());
    }
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator Move()
    {
        while (true)
        {
            if (status == 0)
            {
                yield return new WaitForSeconds(stopTime);
                status = 1;
                targetPosiotion = startPosition + movement;
            }

            else if (status == 1)
            {
                var distance = targetPosiotion - transform.position;
                if (distance.magnitude > 0.01f)
                {
                    transform.position += distance.normalized * 0.05f;
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    status = 2;
                }
            }

            else if (status == 2) {
                yield return new WaitForSeconds(stopTime);
                status = 3;
                targetPosiotion = startPosition;
            }

            else
            {
                var distance = targetPosiotion - transform.position;
                if (distance.magnitude > 0.01f)
                {
                    transform.position += distance.normalized * 0.05f;
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    status = 0;
                }
            }

        }
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(410, 10, 200, 20), "status:" + status);
        GUI.TextField(new Rect(410, 30, 200, 20), "Time.deltaTime" + Time.deltaTime);
    }
}
