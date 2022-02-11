using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplashEffectCall : MonoBehaviour
{
    public UnityEvent<Transform> OnCollision = new UnityEvent<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        OnCollision.Invoke(transform);
    }
}
