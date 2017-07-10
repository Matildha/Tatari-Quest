using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Player player;

    public float speed = 2.5f;
    public float mouseSensitivity = 2.0f;
    public float fearIncrease = 0.5f;

    public bool unableToMove;

    GameObject playerCamera;
    
    float rotationX = 0f;

	void Start ()
    {
        this.playerCamera = this.transform.Find("Player Camera").gameObject;
        unableToMove = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!unableToMove)
        {
            float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
            float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

            if (!player.lantern.isLit && (moveLR != 0 || moveFB != 0))
            {
                player.fearMeter.ChangeFear(fearIncrease * Time.deltaTime);
            }

            float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 20);
            playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

            transform.Translate(moveLR * Time.deltaTime, 0, moveFB * Time.deltaTime);
            transform.Rotate(0, rotationY, 0);
        }
    }
}