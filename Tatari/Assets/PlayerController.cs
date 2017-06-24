﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2.5f;
    public float mouseSensitivity = 2.0f;
    GameObject playerCamera;
    float rotationX = 0f;

	// Use this for initialization
	void Start () {
        this.playerCamera = this.transform.Find("Player Camera").gameObject;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        float moveLR = Input.GetAxis("Horizontal") * speed; // Left, right movement
        float moveFB = Input.GetAxis("Vertical") * speed;  // Front, back movement

        float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * mouseSensitivity, -20, 20);
        //playerCamera.gameObject.transform.localEulerAngles.Set(Mathf.Clamp(rotationX, -60, 60), 0, 0);
        //playerCamera.gameObject.transform.localEulerAngles.Set(-rotationX, 0, 0);
        playerCamera.gameObject.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

        transform.Translate(moveLR * Time.deltaTime, 0, moveFB * Time.deltaTime);
        transform.Rotate(0, rotationY, 0);
        //playerCamera.gameObject.transform.Rotate(-rotationX, 0, 0);

        if(Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
