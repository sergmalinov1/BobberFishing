using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Sprite[] Values;
    public Image ProgressBarImage;
    public float WaitingSecondsBetweenAnimation = 0.01f;
    private int currentValue;


    private void Start()
    {
        ProgressBarImage.sprite = Values[0];
        currentValue = 0;
    }

    public void SetProgress(float progressValue)
    {
            int value = (int)progressValue;
            if (value <= Values.Length)
            {
                StartCoroutine(DrawNewValue(value));
            }
    }


    private void DrawProgressBar(int newValue)
    {
        ProgressBarImage.sprite = Values[newValue];
    }


    private IEnumerator DrawNewValue(int newValue)
    {
        if (newValue > currentValue)
        {
            for (int i = currentValue; i <= newValue; i++)
            {
                DrawProgressBar(i);
                yield return new WaitForSeconds(WaitingSecondsBetweenAnimation);
            }
        } else if(newValue < currentValue)
        {
            for (int i = currentValue; i >= newValue; i--)
            {
                DrawProgressBar(i);
                yield return new WaitForSeconds(WaitingSecondsBetweenAnimation);
            }
        }

        currentValue = newValue;
    }

}
