using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchedFishInfo : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _fishNameText;

    [SerializeField]
    private TMPro.TMP_Text _fishWeightText;

    [SerializeField]
    private TMPro.TMP_Text _fishSummText;

    [SerializeField]
    private TMPro.TMP_Text _fishLengthText;

    [SerializeField]
    private Canvas _canvas;

    public void ShowInfo()
    {
        _canvas.enabled = true;
    }

    public void HideInfo()
    {
        _canvas.enabled = false;
    }


    public void SetValuesAndShow(string FishName, float FishWeight,float SumCatched,float FishLenght)
    {
        _fishNameText.text = FishName;
        _fishWeightText.text = FormatWeight(FishWeight) + " kg";
        _fishLengthText.text = FormatSumm(FishLenght) + " cm";
        _fishSummText.text = FormatSumm(SumCatched);
        ShowInfo();
    }

    private string FormatWeight(float weight)
    {
        return string.Format("{0:F1}", weight);
    }

    private string FormatSumm(float summ)
    {
        return string.Format("{0}", (int)summ);
    }
}
