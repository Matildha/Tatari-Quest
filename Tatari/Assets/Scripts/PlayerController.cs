using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //public Player player;
    //public GameObject playerBody;

    float speed = 3f;
    //public float bounce = 2f;
    float mouseSensitivity = 1.2f;
    public float fearIncrease = 0.75f;

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

        float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 10);
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

        float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
        float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

        if ((moveLR != 0 || moveFB != 0))
        {
            if (!GetComponent<Player>().lantern.isLit)
            {
                GetComponent<Player>().fearMeter.ChangeFear(fearIncrease * Time.deltaTime);  
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Trigger in player controller");
    }
}