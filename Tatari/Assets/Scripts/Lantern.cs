using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour {

    public bool isLit;

    GameObject lanternLight;
    Material lanternGlow;
    Color defaultColor;

    
    private void Start()
    {
        lanternLight = this.transform.Find("Lantern Light").gameObject;
        lanternGlow = GetComponent<Renderer>().material;
        defaultColor = lanternGlow.GetColor("_EmissionColor");
        lanternGlow.SetColor("_EmissionColor", Color.black);  //TODO: Remove or change later!!
        isLit = false;
    }

    public void ToggleLight()
    {
        lanternGlow.EnableKeyword("_EMISSION");
        if (isLit)
        {
            lanternLight.SetActive(false);
            lanternGlow.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            lanternLight.SetActive(true);
            lanternGlow.SetColor("_EmissionColor", defaultColor);
        }
        isLit ^= true;  // xor
    }
}
