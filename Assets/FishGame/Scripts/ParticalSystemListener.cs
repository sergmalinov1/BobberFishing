using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticalSystemListener : MonoBehaviour
{

    public UnityEvent OnParticalStop;

    private void OnParticleSystemStopped()
    {
        OnParticalStop.Invoke();
    }

}
