using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TropheiItem : MonoBehaviour
{
    public Sprite Sadok0;
    public Sprite Sadok2;
    public Sprite Sadok7;
    public Sprite Sadok10;

    public Sprite CoinSprite;

    public Image SadokImage;
    public Image FishImage;
    public Image CoinImage;
    public TMPro.TMP_Text TextWeightKG;
    public TMPro.TMP_Text TextWeightValue;
    public TMPro.TMP_Text TextSummtValue;
    public Button ButtonSale;

    public AnimationCurve XCurve, YCurve;

    private Vector3 StarCointPosition;
    private FishData _fishData;
    private SaveData preferences;
    private TropheyModel tropheyModel;
    private int _sumSell = 0;

    private SaveDataObject savedDataObject;

    void Start()
    {
        StarCointPosition = CoinImage.transform.position;
        savedDataObject = FindObjectOfType<SaveDataObject>();
    }

    private IEnumerator StartCoinAnimation()
    {
        CoinImage.gameObject.SetActive(true);

        Vector3 StartPosition = StarCointPosition;
        Vector3 EndPosition = new Vector3(Screen.width / 2, 0f, 0f);

        for (float t = 0; t < 1; t += Time.deltaTime / 2f)
        {
            float xt = XCurve.Evaluate(t);
            float yt = YCurve.Evaluate(t);
            float x = Mathf.LerpUnclamped(StartPosition.x, EndPosition.x, xt);
            float y = Mathf.LerpUnclamped(StartPosition.y, EndPosition.y, yt);

            CoinImage.gameObject.transform.position = new Vector3(x, y, 0f);
            yield return null;
        }

        CoinImage.gameObject.SetActive(false);
        CoinImage.transform.position = StarCointPosition;

        if(tropheyModel)
        {
            tropheyModel.CoinStopAnimation();
        }

    }



    public void ButtomSellClick()
    {

        if (preferences != null)
        {
            preferences.m_Score += _sumSell;

            for (int indexTropheyInfo = 0; indexTropheyInfo < preferences.TropheyInfo.Count; indexTropheyInfo++)
            {
                List<float> currentElement = preferences.TropheyInfo[indexTropheyInfo];
                if (currentElement[0] == _fishData.FishID)
                {
                    currentElement[1] = 0f;
                    currentElement[2] = 0f;
                    savedDataObject.saveGameData();
                    Recalc();
                    break;
                }
            }
        }

        StartCoroutine(StartCoinAnimation());
    }

    public void Init(FishData fishData, TropheyModel trophModel,SaveData pref)
    {
        _fishData = fishData;
        tropheyModel = trophModel;
        preferences = pref;
        Recalc();
    }

    public void Recalc()
    {

        if(preferences == null)
        {
            return;
        }


        if (TextWeightKG)
        {
            TextWeightKG.gameObject.SetActive(false);
        }
        if (TextWeightValue)
        {
            TextWeightValue.gameObject.SetActive(false);
        }
        if (ButtonSale)
        {
            ButtonSale.gameObject.SetActive(false);
        }
        if (CoinImage)
        {
            CoinImage.gameObject.SetActive(false);
        }

        bool fishExists = false;
        List<float> currentElement = new List<float>();
        if (_fishData)
        {
            for (int indexTropheyInfo = 0; indexTropheyInfo < preferences.TropheyInfo.Count; indexTropheyInfo++)
            {
                currentElement = preferences.TropheyInfo[indexTropheyInfo];
                if (currentElement[0] == _fishData.FishID)
                {
                    fishExists = true;
                    break;
                }
            }

            if(fishExists)
            {
                FishImage.sprite = _fishData.TropheyColorPhoto;
                UpdateSadok(currentElement[1]);
                updateSaleButton(currentElement[2]);
            }
            else
            {
                FishImage.sprite = _fishData.TropheyBlackPhoto;
                UpdateSadok(0);
            }

        }
    }


    private void UpdateSadok(float WeightCatched)
    {

        if (WeightCatched > 0)
        {
            TextWeightKG.gameObject.SetActive(true);
            TextWeightValue.gameObject.SetActive(true);
            if (WeightCatched > 0f)
            {
                float WeightCatchedValue = WeightCatched / 1000f;
                TextWeightValue.text = FormatWeight(WeightCatchedValue);
            }
        }


        if (WeightCatched < 2000f)
        {
            SadokImage.sprite = Sadok0;
            return;
        }

        if (WeightCatched >= 2000f && WeightCatched < 7000f)
        {
            SadokImage.sprite = Sadok2;
            return;
        }

        if (WeightCatched >= 7000f && WeightCatched < 10000f)
        {
            SadokImage.sprite = Sadok7;
            return;
        }

        if (WeightCatched >= 10000f)
        {
            SadokImage.sprite = Sadok10;
        }

    }

    private void updateSaleButton(float SumSell)
    {
        if (SumSell > 0)
        {
            if (CoinImage)
            {
                CoinImage.gameObject.SetActive(true);
            }

            if (ButtonSale)
            {
                ButtonSale.gameObject.SetActive(true);
            }

            if (TextSummtValue)
            {
                TextSummtValue.text = FormatSumm(SumSell);
            }
        }
        _sumSell = (int) SumSell;
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
