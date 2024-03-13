using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Noir Project/Simple Gauge Maker/Gauge Sample Value Modifier")]
public class gaugeValueSample : MonoBehaviour
{

    public Transform SpeedGaugeTransform = null;
    private SimpleGaugeMaker speedGauge = null;


    public float speedMultiplier = 40.0f;
    private float _tempValue = 0;
    private float currentSpeed = 0;
    private bool reverse = false;

    public float rpmMultiplier = 20.0f;
    private float _tempRPM = 0;
    private float currentRPM = 0;
    private bool reverseRPM = false;

    // Use this for initialization
    void Start()
    {
        if (SpeedGaugeTransform != null)
        {
            if (SpeedGaugeTransform.GetComponent<SimpleGaugeMaker>() != null)
            {
                speedGauge = SpeedGaugeTransform.GetComponent<SimpleGaugeMaker>();
            }
        }
    }

    void rpmModifier()
    {
        //this will increase and reduce the number by time and time rpm Multiplier 
        if (!reverseRPM)
        {
            //increase the value by time multuiplier when reverse is false.
            currentRPM += rpmMultiplier * Time.deltaTime;
        }
        else
        {
            //vice versa, reduce the value when reverse is true.
            currentRPM -= rpmMultiplier * Time.deltaTime;
        }

        //when the value is equal or over the 120 set the rever on
        if (currentRPM >= 8000)
            reverseRPM = true;

        //when the speed is equal or under 0 set re reverse off
        if (currentRPM <= 0)
            reverseRPM = false;

        //this line send the current value to the gauge desired input
        speedGauge.setInputValue("RPM", currentRPM);
    }

    // Update is called once per frame
    void Update()
    {
        //this will increase and reduce the number by time and time speed Multiplier 
        if (!reverse)
        {
            //increase the value by time multuiplier when reverse is false.
            currentSpeed += speedMultiplier * Time.deltaTime;
        } else {
            //vice versa, reduce the value when reverse is true.
            currentSpeed -= speedMultiplier * Time.deltaTime;
        }

        //when the value is equal or over the 120 set the rever on
        if (currentSpeed >= 120)
            reverse = true;

        //when the speed is equal or under 0 set re reverse off
        if (currentSpeed <= 0)
            reverse = false;

        //this line send the current value to the gauge desired input
        speedGauge.setInputValue("Speed", currentSpeed);
        speedGauge.setTextLayerTag(1, "X", false);


        //similar to speed just change RPM gauge
        rpmModifier();
    }
}
