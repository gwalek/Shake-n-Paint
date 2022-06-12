using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShakeDetection : MonoBehaviour
{
    // Huge thanks to Epitome for an example!
    // https://www.youtube.com/watch?v=dO_YrCV7uk8

    // Grash's Notes: 
    // Made this a singleton, and didn't use anything related to it!
    public static ShakeDetection instance;

    // Set this in the Inspector... 
    public float shakeDetectionThreshold = 2.0f;
    private float _shakeDetectionThreshold; 
    public float ShakeDetectionThreshold
    {
        set { _shakeDetectionThreshold = value * value; }
        get { return _shakeDetectionThreshold;  }
    }

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
        // Grash's notes
        // Epitome chose to go this by using sqrMagnitude for speed. 
        // And Did it in place, I've gone back and made this into a property 
        // With something to assign in the Editor for it. 

        ShakeDetectionThreshold = shakeDetectionThreshold;

        lowPassFilterFactor = updateInterval / lowPassKernalWidthInSeconds;
        lowPassValue = Input.acceleration;
    }


    void Update()
    {
        Vector3 accel = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, accel, lowPassFilterFactor);
        Vector3 deltaAccel = accel - lowPassValue; 

        if (deltaAccel.sqrMagnitude >= ShakeDetectionThreshold)
        {
            OnShake?.Invoke(); 
        }
    }
}
