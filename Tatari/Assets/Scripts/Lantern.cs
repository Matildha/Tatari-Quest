using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour {

    GameObject lanternLight;
    bool lightToggle = true;
    Material lanternGlow;
    Color defaultColor;

    private void Start()
    {
        lanternLight = this.transform.Find("Lantern Light").gameObject;
        lanternGlow = GetComponent<Renderer>().material;
        defaultColor = lanternGlow.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown("space"))
        {
            lanternLight.SetActive(!lightToggle);
            lanternGlow.EnableKeyword("_EMISSION");
            if (lightToggle)
                lanternGlow.SetColor("_EmissionColor", Color.black);
            else
                lanternGlow.SetColor("_EmissionColor", defaultColor);
            lightToggle ^= true; // xor
            print("Pressed space");
        }
		
	}
}
