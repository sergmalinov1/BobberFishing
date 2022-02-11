using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckVisible : MonoBehaviour
{
    public UnityEvent OnVisible;
    public UnityEvent OnInVisible;

    private void OnBecameVisible()
    {
        OnVisible.Invoke();
    }

    private void OnBecameInvisible()
    {
        OnInVisible.Invoke();
    }
}
