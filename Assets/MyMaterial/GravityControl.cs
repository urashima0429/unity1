using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour {

    public float weight = 10f;
    public Vector3 velocity;
    public Vector3 prevVelocity;
    public bool isCollided = false;

    private GameObject collidedObject;
    private static float coefficiantConstant = 0.5f;
    private static float deltaTime = 1000;
    private Vector3 acceleration = new Vector3(0,0,0);
    private GameObject[] stars;

    // Use this for initialization
    void Start () {
		collidedObject = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollided)
        {
            float otherWeight = collidedObject.GetComponent<GravityControl>().weight;
            Vector3 otherVelocity = collidedObject.GetComponent<GravityControl>().prevVelocity;
            velocity = (coefficiantConstant * otherWeight * (otherVelocity - velocity) + (weight * velocity) + (otherWeight * otherVelocity)) / (weight + otherWeight);
            isCollided = false;
        }

        else
        {
            stars = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < stars.Length; i++)
                {
                    if (this != stars[i])
                    {
                        Vector3 starDirection = stars[i].transform.position - transform.position;
                        float starWeight = stars[i].GetComponent<GravityControl>().weight;
                        acceleration += (starWeight / Mathf.Pow(starDirection.magnitude, 3)) * starDirection;
                    }
                }

            velocity += acceleration;
            transform.position += velocity * (Time.deltaTime / deltaTime);
            
        }

    }

    void OnTriggerEnter(Collider other) {
        isCollided = true;
        prevVelocity = velocity;
        collidedObject = other.gameObject;
    }

    void OnGUI (){
        GUI.TextField(new Rect(10, 10, 200, 20), "accelaration:" + acceleration);
        GUI.TextField(new Rect(10, 30, 200, 20), "velocity:" + velocity);
        GUI.TextField(new Rect(10, 50, 200, 20), "position.x:" + transform.position.x);
        GUI.TextField(new Rect(10, 70, 200, 20), "position.y:" + transform.position.y);
        GUI.TextField(new Rect(10, 90, 200, 20), "position.z:" + transform.position.z);
    }
}
