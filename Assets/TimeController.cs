﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {


    public static float time;

	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(610, 10, 200, 20), "time:" + time); 
    }
}
