using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Donate : MonoBehaviour
{
    private static GameObject instance;
    private SaveDataObject _saveDataObject;

    public int SumDonate = 10;

    public UnityEvent OnDonateSuccessfull;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        _saveDataObject = FindObjectOfType<SaveDataObject>();
    }


    public void StartDonate()
    {
        DonateSuccessfull();
        //_adsManager.ShowReward(); //
    }


    public void DonateSuccessfull()
    {
        _saveDataObject.GetSaveData().m_Score += SumDonate;
        _saveDataObject.saveGameData();
        OnDonateSuccessfull.Invoke();
    }



}
