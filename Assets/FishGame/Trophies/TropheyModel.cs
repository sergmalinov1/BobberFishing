using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TropheyModel : MonoBehaviour
{

    public TMPro.TMP_Text textScoreValue;
    public Transform TropheyScrollContentTransform;
    public GameObject Prefab;
    private SaveData preferences;
    private List<GameObject> _listItems = new List<GameObject>();
    private SaveDataObject savedDataObject;
    private Donate _donate;


    private LanguageDictionary _languageDictionary;
    private GamesObjectsDictionary gamesObjectsDictionary;

    public TMPro.TMP_Text TextCaption;

    private void Start()
    {
        _donate = FindObjectOfType<Donate>();
        _donate.OnDonateSuccessfull.AddListener(updateScore);

        gamesObjectsDictionary = FindObjectOfType<GamesObjectsDictionary>();

        savedDataObject = FindObjectOfType<SaveDataObject>();
        preferences = savedDataObject.GetSaveData();
        _languageDictionary = FindObjectOfType<LanguageDictionary>();
        updateScore();

        List<FishData> listFish = gamesObjectsDictionary.GetFishList();
        for (int i = 0; i < listFish.Count; i++)
        {

            Debug.Log(listFish[i].name);

            GameObject newItem = Instantiate(Prefab, TropheyScrollContentTransform);
            TropheiItem tropheiItem = newItem.GetComponent<TropheiItem>();

            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y,i);

            if (tropheiItem)
            {
                tropheiItem.Init(listFish[i],this,preferences);
            } 
            _listItems.Add(newItem);
        }
        UpdateItems();
    }


    private void UpdateItems()
    {
        for (int i = 0; i < _listItems.Count; i++)
        {
            _listItems[i].GetComponent<TropheiItem>().Recalc();
        }

        TextCaption.text = _languageDictionary.GetWord("T R O P H E Y");
    }


    private void updateScore()
    {
        if (textScoreValue != null)
        {
            textScoreValue.text = preferences.m_Score.ToString();
        }
    }

    public void CoinStopAnimation()
    {
        updateScore();
    }

    public void BackClick()
    {
        SceneManager.LoadScene("mainScene");
    }

    public void DonateClick()
    {
        _donate.StartDonate();
    }
}
