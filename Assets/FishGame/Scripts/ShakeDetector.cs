using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShakeDetector : MonoBehaviour
{

    public float ShakeDetectionThreshold; //3.6
    public float MinShakeInterval;        //0.2
    public float ShakeForce;              //5

    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;

    public UnityEvent OnShakeDetect;

    void Start()
    {
        sqrShakeDetectionThreshold = Mathf.Pow(ShakeDetectionThreshold, 2);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold && Time.unscaledTime >= timeSinceLastShake + MinShakeInterval)
        {
            timeSinceLastShake = Time.unscaledTime;
            //GameViewModel.ShakeDetected();
            OnShakeDetect.Invoke();
        }

    }
}
