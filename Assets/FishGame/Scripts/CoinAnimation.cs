using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{

    public AnimationCurve XCurve, YCurve;
    public TropheyModel tropheyModel;

    private IEnumerator StartCoinAnimation()
    {
        Vector3 StartPosition = transform.position;
        Vector3 EndPosition = new Vector3(Screen.width / 2,0f,0f);

        Debug.Log(StartPosition);

        for(float t = 0; t < 1; t+= Time.deltaTime)
        {
            float xt = XCurve.Evaluate(t);
            float yt = YCurve.Evaluate(t);
            float x = Mathf.LerpUnclamped(StartPosition.x, EndPosition.x, xt);
            float y = Mathf.LerpUnclamped(StartPosition.y, EndPosition.y, yt);

            transform.position = new Vector3(x, y, 0f);
            yield return null;
        }

        gameObject.SetActive(false);
        if (tropheyModel != null)
        {
            tropheyModel.CoinStopAnimation();
        }

    }

    public void StartAnimation()
    {
        StartCoroutine(StartCoinAnimation());
    }
}
