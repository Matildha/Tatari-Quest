using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool toBeDisabled;

    public float speed = 1f;
    public Vector3 motion;
    Vector3 startPos;
    int targetPosition;  // Will be 1 if door is on startPos and -1 if door is on startPos + motion
    bool isMoving;
    bool hasPlayedSound;

    bool isOpen;
    float openDuration = 3f;
    float openStart;

    const string PROMPT_MSG = "Press 'E' to move door";
    
   
    private void Start()
    {
        targetPosition = 1;
        startPos = transform.localPosition;
    }

    public override string PromptMessage { get { return PROMPT_MSG; } }

    public override void Interact()
    {
        isMoving = true;
        //isOpen = true;
        openStart = Time.time;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
                                                            startPos + (motion * targetPosition),
                                                            speed * Time.deltaTime);
            if(!hasPlayedSound)
            {
                transform.parent.GetComponent<AudioSource>().Play();
                hasPlayedSound = true;
            }
        }
        if (transform.localPosition == startPos + (motion * targetPosition))
        {
            isMoving = false;
            targetPosition *= -1;
            isOpen = !isOpen;
            startPos = transform.localPosition;
            transform.parent.GetComponent<AudioSource>().Stop();
            hasPlayedSound = false;

            if (toBeDisabled) enabled = false;
        }

        if(isOpen && Time.time - openStart > openDuration)
        {
            isMoving = true;  // So the door is closed
            //isOpen = false;
        }

        keepMeEnabled = isMoving || isOpen;
    }
}