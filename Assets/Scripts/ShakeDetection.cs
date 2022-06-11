using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShakeDetection : MonoBehaviour
{
    public static ShakeDetection instance;

    public float shakeDetectionThreshold = 2.0f;
    public float updateInterval = (1.0f / 60.0f);
    public float lowPassKernalWidthInSeconds = 1.0f;
    public float lowPassFilterFactor;
    public Vector3 lowPassValue;

    [Space(10)]
    public UnityEvent OnShake; 


    void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        lowPassFilterFactor = updateInterval / lowPassKernalWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }


    void Update()
    {
        Vector3 accel = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, accel, lowPassFilterFactor);
        Vector3 deltaAccel = accel - lowPassValue; 

        if (deltaAccel.sqrMagnitude >= shakeDetectionThreshold)
        {
            OnShake?.Invoke(); 
        }
    }
}
