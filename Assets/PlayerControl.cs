using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float speed = 5.0f;
    public float rotateSpeed = 10.0f;
    public float jumpSpeed = 30.0f;
    public float stopSpeed = 0.3f;
    public float decelerationRate = 0.2f;
    public float tolerance = 1f;
    public Vector3 startPosition = new Vector3(0, 0, 1);
    public LayerMask ground;
    public LayerMask inWater;
    private Rigidbody playerRB;
    private bool isGround;
    public GameObject playerAxis;
    public float outsideHeight;

    // Use this for initialization
    void Start() {
        playerRB = this.GetComponent<Rigidbody>();
        playerAxis = GameObject.Find("PlayerAxis");
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        //移動入力なし
        isGround = CheckGrounded();
        if (Input.GetAxis("Vertical")== 0  && Input.GetAxis("Horizontal") == 0)
        {
                //減速
                playerRB.velocity -= decelerationRate * playerRB.velocity;

                //一定速度以下で停止
                if (playerRB.velocity.magnitude < stopSpeed)
                {
                    playerRB.velocity -= new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
                }
        }
        
        //移動入力あり
        else
        {
            playerRB.velocity += Input.GetAxis("Vertical") * playerAxis.transform.forward * speed * Time.deltaTime;
            playerRB.velocity += Input.GetAxis("Horizontal") * playerAxis.transform.right * speed * Time.deltaTime;
        }

        
        if (isGround)
        {

        }else
        {
            if (CheckOutside())
            {
                Restart();
            }
        }

    }

    public bool CheckGrounded()
    {
        var ray = new Ray(playerRB.position, Vector3.down);
        return Physics.Raycast(ray, tolerance, ground);
    }

    public bool CheckOutside()
    {
        return transform.position.y < outsideHeight;
    }

    public void Restart()
    {
        transform.position = startPosition;
        playerRB.velocity = Vector3.zero;
    }

    public void teleportate(Vector3 position)
    {
        transform.position = position;
        playerRB.velocity = Vector3.zero;
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 200, 20), "playerRB.velocity.x:" + playerRB.velocity.x);
        GUI.TextField(new Rect(10, 30, 200, 20), "playerRB.velocity.y:" + playerRB.velocity.y);
        GUI.TextField(new Rect(10, 50, 200, 20), "playerRB.velocity.z:" + playerRB.velocity.z);
        GUI.TextField(new Rect(10, 70, 200, 20), "playerRB.position.x:" + playerRB.position.x);
        GUI.TextField(new Rect(10, 90, 200, 20), "playerRB.position.y:" + playerRB.position.y);
        GUI.TextField(new Rect(10, 110, 200, 20), "playerRB.position.z:" + playerRB.position.z);
        GUI.TextField(new Rect(10, 130, 200, 20), "check" + playerRB.velocity.magnitude);
        GUI.TextField(new Rect(10, 150, 200, 20), "isGround:" + isGround);
    }
}
