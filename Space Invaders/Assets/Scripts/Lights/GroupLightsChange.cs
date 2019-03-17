using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupLightsChange : MonoBehaviour
{

    public Component[] lights;
    [SerializeField]bool on;
    bool changing;
    float time = 0.0f;
    [SerializeField] float maxIntensity = 1.0f;


    void Start()
    {
        lights = GetComponentsInChildren<Light>(true);
        if (on) foreach (Light l in lights) l.intensity = maxIntensity;
        else foreach (Light l in lights) l.intensity = 0.0f;

        //Fade(5.0f); //Quitable

    }

    void Update()
    {
        if (changing)
        {
            changeIntensity();
        }
    }

    void changeIntensity()
    {
        foreach (Light l in lights)
        {
            if (on) l.intensity += (1 / time) * Time.deltaTime;
            else l.intensity -= (1 / time) * Time.deltaTime;
            if(l.intensity > maxIntensity)
            {
                changing = false;
                l.intensity = maxIntensity;
            }
        }
    }

    public void Fade(float seconds)
    {
        on = !on;
        changing = true;
        time = seconds;
    }
}
