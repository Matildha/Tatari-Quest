using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //public Player player;
    //public GameObject playerBody;

    public float speed = 2.5f;
    public float bounce = 2f;
    public float mouseSensitivity = 2.0f;
    public float fearIncrease = 0.5f;

    public bool unableToMove;

    GameObject playerCamera;
    
    float rotationX = 0f;

    Vector3 safeSpot;
    bool movingToSafe;

	void Start ()
    {
        this.playerCamera = this.transform.Find("Player Camera").gameObject;
        unableToMove = false;
        movingToSafe = false;
    }

    private void Update()
    {
        float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 20);
        playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);
        transform.Rotate(0, rotationY, 0);
    }


    void FixedUpdate ()
    {
        if (!unableToMove)
        {
            float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
            float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

            if (!GetComponent<Player>().lantern.isLit && (moveLR != 0 || moveFB != 0))
            {
                GetComponent<Player>().fearMeter.ChangeFear(fearIncrease * Time.deltaTime);
            }

            Vector3 direction = new Vector3(moveLR * Time.deltaTime, 0,
                                                                moveFB * Time.deltaTime);

            GetComponent<Rigidbody>().MovePosition(transform.TransformDirection(direction) + transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Collision in player controller");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger in player controller");
    }
}