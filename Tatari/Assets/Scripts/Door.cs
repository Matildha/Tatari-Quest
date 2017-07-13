using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public float speed = 1f;
    public Vector3 motion;
    Vector3 startPos;
    int targetPosition;  // Will be 1 if door is on startPos and -1 if door is on startPos + motion
    bool isMoving;

    const string PROMPT_MSG = "Press E to move door";
    
   
    private void Start()
    {
        targetPosition = 1;
        startPos = transform.localPosition;
    }

    public override string PromptMessage { get { return PROMPT_MSG; } }

    public override void Interact()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
                                                            startPos + (motion * targetPosition),
                                                            speed * Time.deltaTime);
        }
        if (transform.localPosition == startPos + (motion * targetPosition))
        {
            isMoving = false;
            targetPosition *= -1;
        }
    }
}