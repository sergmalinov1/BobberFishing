using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefineChance : MonoBehaviour
{
    public float Chanse = 0;
    public float PeriodUpdateSec = 1f;
    private float _lastUpdateTime = 0f;

    public UnityEvent<float> OnChanceChanged = new UnityEvent<float>();

    void Update()
    {
        /*
        if (Time.time - _lastUpdateTime >= 1.0f)
        {
            //Debug.Log("*********************Chanse = " + Chanse + " ********************************");
            _lastUpdateTime = Time.time;
            OnChanceChanged.Invoke(Chanse);
        }
        */
        _lastUpdateTime += Time.deltaTime;

        if (_lastUpdateTime > PeriodUpdateSec)
        {
            OnChanceChanged.Invoke(Chanse);
            _lastUpdateTime = 0f;
        }

    }
}
