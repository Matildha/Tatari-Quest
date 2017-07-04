using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2.5f;
    public float mouseSensitivity = 2.0f;

    GameObject playerCamera;
    
    float rotationX = 0f;

	void Start () {
        this.playerCamera = this.transform.Find("Player Camera").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
        float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

        float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -40, 20);
        playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

        transform.Translate(moveLR * Time.deltaTime, 0, moveFB * Time.deltaTime);
        transform.Rotate(0, rotationY, 0);
    }
}