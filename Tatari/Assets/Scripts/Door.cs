using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable{

    public int worldAreaID;

    public float speed = 1f;
    //Set with Unity interface  
    public Vector3[] positions = { new Vector3(-4.52f, 2.13f, -6.82f),             
                                    new Vector3(-4.58f, 2.13f, -3.95f) };  // Two possible positions for the door (SIZE 2)

    string promptMsg = "Press E to move the door";  
    int targetPosition;
    bool isMoving;

    private void Start()
    {
        targetPosition = 1;
    }

    public override string PromptMessage { get { return promptMsg; } }

    public override void Interact()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                                positions[targetPosition], speed * Time.deltaTime);
        }
        if (transform.localPosition == positions[targetPosition])
        {
            isMoving = false;
            targetPosition = (++targetPosition) % 2;
        }
    }
}