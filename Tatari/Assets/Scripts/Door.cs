using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Door inherits form Interactable and starts to move its transform
 * at the Interact() call. The direction and distance of 
 * the movement is determined by Vector3 "motion". 
 * 
 * Door is automatically reset to its originally positioned after
 * "OPEN_DURATION" seconds, if Interact() has not been called again
 * before that. 
 * 
 * Door sets its "keepMeEnabled" flag to ensure changing of active
 * world area (see classes WorldManager and WorldArea) does not 
 * interrupt its ongoing movement. 
*/

public class Door : Interactable
{
    public bool toBeDisabled;  // Enables InteractableManager to tell Door that it should disabled itself after
                                // finishing its current movement

    public float speed = 1f;
    public Vector3 motion;
    Vector3 startPos;
    int targetPosition;  // Will be 1 if door is on startPos and -1 if door is on startPos + motion
    bool isMoving;
    bool hasPlayedSound;

    bool isOpen;
    const float OPEN_DURATION = 3f;
    float openStart;

    const string PROMPT_MSG = "Press 'E' to move door";

    public override string PromptMessage { get { return PROMPT_MSG; } }


    private void Start()
    {
        targetPosition = 1;
        startPos = transform.localPosition;
    }

    public override void Interact()
    {
        isMoving = true;
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
        // Check if reached target destination
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

        if(isOpen && Time.time - openStart > OPEN_DURATION)
        {
            isMoving = true;        // Trigger movement towards original position
        }

        keepMeEnabled = isMoving || isOpen;
    }
}
