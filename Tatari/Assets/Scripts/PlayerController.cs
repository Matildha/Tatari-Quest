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
        //GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }

    /*void Update()
    {
        if (!unableToMove)
        {
            float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
            float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

            if (!GetComponent<Player>().lantern.isLit && (moveLR != 0 || moveFB != 0))
            {
                GetComponent<Player>().fearMeter.ChangeFear(fearIncrease * Time.deltaTime);
            }

            float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 20);
            playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

            Vector3 newPos = new Vector3(moveLR * Time.deltaTime, 0, moveFB * Time.deltaTime) + transform.position;

            CapsuleCollider caps = GetComponent<CapsuleCollider>();
            Vector3 p1 = transform.position + caps.center + Vector3.up * -caps.height * 0.5f;
            Vector3 p2 = p1 + Vector3.up * caps.height;

            //Vector3 direction = Vector3.Normalize(newPos - transform.position);
            Vector3 direction = newPos - transform.position;
            RaycastHit hit;
            transform.Rotate(0, rotationY, 0);

            Rigidbody body = GetComponent<Rigidbody>();


            /*if (!Physics.CapsuleCast(p1, p2, caps.radius, direction, out hit, 0.25f))
            {
                transform.Translate(moveLR * Time.deltaTime, 0, moveFB * Time.deltaTime);
                
                safeSpot = transform.position + bounce * -direction;
            }
            else
            {
                print("Capsule cast hit");
                movingToSafe = true;
                //transform.Translate(-moveLR * Time.deltaTime + bounce * -direction.x, 0, -moveFB 
                  //                                                              * Time.deltaTime + bounce * - direction.z);
                //Vector3.MoveTowards(transform.position, safeSpot, speed * Time.deltaTime);
                transform.position = safeSpot;
                movingToSafe = false;
                if(transform.position == safeSpot) movingToSafe = false;
            }

        }
    }*/

    private void Update()
    {
        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 20);
        playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);
        //Quaternion quatRotY = Quaternion.AngleAxis(rotationY * Time.deltaTime, Vector3.up);
        //GetComponent<Rigidbody>().MoveRotation(quatRotY);
        transform.Rotate(0, rotationY, 0);
        //GetComponent<Rigidbody>().MoveRotation()
    }


    void FixedUpdate ()
    {
        if (!unableToMove)
        {
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            

            float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
            float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

            if (!GetComponent<Player>().lantern.isLit && (moveLR != 0 || moveFB != 0))
            {
                GetComponent<Player>().fearMeter.ChangeFear(fearIncrease * Time.deltaTime);
            }

            float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
            //rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 20);
            //playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

            //Vector3 newPos = new Vector3(moveLR * Time.deltaTime, 0, 
                                                                //moveFB * Time.deltaTime) + transform.position;

            Vector3 direction = new Vector3(moveLR * Time.deltaTime, 0,
                                                                moveFB * Time.deltaTime);

            Quaternion quatRotY = Quaternion.AngleAxis(rotationY * 
                                                                GetComponent<Rigidbody>().rotation.y, Vector3.up);
            GetComponent<Rigidbody>().MovePosition(transform.TransformDirection(direction) + transform.position);
            //GetComponent<Rigidbody>().transform.localPosition = transform.TransformDirection(newPos);
            //transform.position = newPos;
            //GetComponent<Rigidbody>().MoveRotation(quatRotY);
            //GetComponent<Rigidbody>().transform.rotation = quatRotY;

            //transform.Translate(moveLR * Time.deltaTime, 0, moveFB * Time.deltaTime);
            //gameObject.transform.Rotate(0, rotationY, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collisison");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //transform.Rotate(0, transform.rotation.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger");
    }
}