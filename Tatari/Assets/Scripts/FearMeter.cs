﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FearMeter : MonoBehaviour {

    public Player player;

    public const float MAX_FEAR = 100f; 
    float fear;

    float maxWidth;
    float height;

    private void Start()
    {
        fear = 0;
        height = transform.GetComponent<RectTransform>().rect.height;
        maxWidth = transform.GetComponent<RectTransform>().rect.width;
        UpdateFearMeter();
    }

    /* Increases or decreases the player's fear. Use negative value to decrement. */
    public void ChangeFear(float delta)
    {
        fear += delta;
        /*if (delta > 5 || delta < -5)*/ // print("Player fear: " + fear);
        if (fear >= MAX_FEAR)
        {
            print("GAMEEEE OOOOOVEEEERRRRR!!!! :O");
            fear = MAX_FEAR;
        }
        UpdateFearMeter();
    }

    private void UpdateFearMeter()
    {
        float width = (fear / MAX_FEAR) * maxWidth;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}