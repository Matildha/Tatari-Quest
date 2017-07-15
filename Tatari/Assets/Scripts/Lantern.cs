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
        lanternLight = transform.Find("Lantern Light").gameObject;
        Renderer lanternRend = transform.Find("Body").GetComponent<Renderer>();
        lanternGlow = lanternRend.material;
        if (lanternGlow == null) print("is null start");
        defaultColor = lanternGlow.GetColor("_EmissionColor");
        print(defaultColor);
        lanternGlow.SetColor("_EmissionColor", Color.black);  //TODO: Remove or change later!!
        isLit = false;
    }

    public void ToggleLight()
    {
        if (lanternGlow == null) print("is null");
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
