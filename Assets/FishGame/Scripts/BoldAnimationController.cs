using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoldAnimationController : MonoBehaviour
{
    public Game gameViewModel;

    public void StopNibble()
    {
        Debug.Log("====STOP ANIMATION====");
        if(gameViewModel != null)
        {
            gameViewModel.StopNibble();
        }
    }
}
