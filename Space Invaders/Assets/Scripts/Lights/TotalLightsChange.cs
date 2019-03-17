using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalLightsChange : MonoBehaviour
{

    public Component[] groupsOfLights;
    float waitingTime;
    float fadeTime;

    void Start()
    {
        groupsOfLights = GetComponentsInChildren<GroupLightsChange>(true);
    }
    public void FadeAllLights(float time = 0.0f, float fade = 2.0f)
    {
        waitingTime = time;
        fadeTime = fade;
        StartCoroutine("WaitForFadeAllLights");
    }
    IEnumerator WaitForFadeAllLights()
    {
        yield return new WaitForSeconds(waitingTime);
        foreach(GroupLightsChange g in groupsOfLights)
        {
            g.Fade(fadeTime);
        }
    }

    
}
