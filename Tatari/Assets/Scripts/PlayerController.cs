using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerController modifies the transform of playerCamera and moves the rigidbody
 * attached to the same gameObject as this script. 
 * 
 * PlayerController manages player movement related input. "unableToMove" can be set
 * to false to disable any movement. 
 * 
 * Variables for speed, mouseSensitivity and fear increase when walking without light 
 * is set in this class. 
 * 
 * PlayerController will set the rigidbody's velocity and angular velocity to zero 
 * on physical collision. 
*/

public class PlayerController : MonoBehaviour {

    const float SPEED = 2.5f;
    const float MOUSE_SENSITIVITY = 1.2f;
    const float FEAR_INCREASE = 0.75f;

    public bool unableToMove;
    public bool isWalking;

    GameObject playerCamera;
    
    float rotationX = 0f;
    

	void Start ()
    {
        this.playerCamera = this.transform.Find("Player Camera").gameObject;
        unableToMove = false;
        isWalking = false;
    }

    public void ExUpdate()
    {
        if (unableToMove) return;

        // Update rotation around y axis for player body and player camera around x axis
        float rotationY = Input.GetAxis("Mouse X") * MOUSE_SENSITIVITY;
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * MOUSE_SENSITIVITY, -40, 10);
        playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);
        transform.Rotate(0, rotationY, 0);
    }


    public void ExFixedUpdate ()
    {
        if (unableToMove)
        {
            isWalking = false;
            return;
        }

        float moveLR = Input.GetAxis("Horizontal") * SPEED; // Left, right movement
        float moveFB = Input.GetAxis("Vertical") * SPEED;  // Front, back movement

        // Update isWalking and call on player FearMeter
        if ((moveLR != 0 || moveFB != 0))
        {
            if (!GetComponent<Player>().lantern.isLit)
            {
                GetComponent<Player>().fearMeter.ChangeFear(FEAR_INCREASE * Time.deltaTime);  
            }
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
          
        Vector3 direction = new Vector3(moveLR * Time.deltaTime, 0,
                                                            moveFB * Time.deltaTime);

        GetComponent<Rigidbody>().MovePosition(transform.TransformDirection(direction) + transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Collision in player controller");
        // Prevent rigidbody from "bouncing" or keep moving after collision
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
