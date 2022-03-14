using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{
    float timeGreen = 15.0f;
    float timeRed = 10.0f;
    float timeLeft;
    public bool light = false;
    private void Start()
    {
        timeLeft = timeGreen;
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 && !light || timeLeft < 5 && light)
        {
            light = !light;
            if (light)
                timeLeft = timeGreen;
            else
                timeLeft = timeRed;
        }
    }
}
