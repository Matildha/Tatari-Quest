using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Lantern controls lanternLights light intensity together with child object "Body"
 * to the gameObject this script is attached to. Intensity of light and emission of 
 * "Body" material is randomly generated and follows a sinus curve. 
 * 
 * Lantern contains functionality to toggle the light and emission property of "Body"'s 
 * material on or off. 
*/

public class Lantern : MonoBehaviour {

    public bool isLit;
    public DemonSpawn demonSpawn;

    GameObject lanternLight;
    Material lanternGlow;
    Color defaultColor;

    float flickerPeriod;
    float flickerX;
    const float FLICKER_MAX_X = 500;
    const float INTENSITY_OFFSET = 2;

    
    private void Start()
    {
        lanternLight = transform.Find("Lantern Light").gameObject;
        Renderer lanternRend = transform.Find("Body").GetComponent<Renderer>();
        lanternGlow = lanternRend.material;
        defaultColor = lanternGlow.GetColor("_EmissionColor");
        lanternGlow.SetColor("_EmissionColor", Color.black);
        isLit = false;
        flickerPeriod = 1;
    }

    public void ToggleLight()
    {
        if (lanternGlow == null) print("is null");
        lanternGlow.EnableKeyword("_EMISSION");
        if (isLit)
        {
            lanternLight.SetActive(false);
            //lanternLight.GetComponent<Light>().intensity = 0;
            lanternGlow.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            lanternLight.SetActive(true);
            //lanternLight.GetComponent<Light>().intensity = 2;
            lanternGlow.SetColor("_EmissionColor", defaultColor);
            demonSpawn.UpdateSafeStartTime();
        }
        isLit ^= true;  // xor
    }

    private void Update()
    {
        if (!isLit) return;

        flickerX++;
        if (flickerX > (2 * Mathf.PI) / flickerPeriod) {
            flickerX = 0;
            Random.InitState(System.DateTime.Now.Millisecond);
            flickerPeriod = Random.Range(0.04f, 0.12f);  // values set from testing
        }
        float lightIntensity = 0.5f * Mathf.Sin(flickerPeriod * flickerX) + INTENSITY_OFFSET;
        lanternLight.GetComponent<Light>().intensity = lightIntensity;
        lightIntensity /= 5;  // value 5 set from testing to match default intensity of defaultColor on lantern
        lanternGlow.SetColor("_EmissionColor", defaultColor + new Color(lightIntensity, lightIntensity, lightIntensity, 1));  // 1 = alpha
    }
}
