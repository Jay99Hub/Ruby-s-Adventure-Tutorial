using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    Timer clock;

    public RectTransform clockHand;

    const float secondsToDegrees = 360 / 30;


    // Start is called before the first frame update
    void Start()
    {
        clock = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        clockHand.rotation = Quaternion.Euler(0, 0, clock.GetSeconds() * secondsToDegrees);
    }
}
