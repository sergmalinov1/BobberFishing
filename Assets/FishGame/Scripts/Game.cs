using Obi;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Image handImage;
    public Animator FishingMainAnimator;
    public Animator FishingFloatAnimator;
    public Animator FishAnimator;
    public Button mainButton;
    public Sprite fishermanSprite;
    public Sprite nextSprite;
    public TMPro.TMP_Text TryCountText;

    public Transform BaitContainer;

    public GameObject MenuCanvas;

    public Vibro Vibro;


    public ParticleSystem EffectMakeBait;
    public GameObject WaterSplashEffect;

    //gameObjects
    private GameObject _bobberObject;
    private GameObject _fishLineObject;
    private GameObject _hookObject;
    private GameObject _hookObjectInfo;
    private GameObject _baitObject;
    private GameObject _baitObjectInfo;
    private LanguageDictionary languageDictionary;


    private bool isIdleState = true;
    private bool isNibbleState = false;
    private Coroutine nibbleCoroutine;


    private float timeBetweenButtonClick = 2f;
    private float timestamp;
    private FishData _catchedFish;
    private GameObject _catchedFishObject;

    public Text DebugText;

    public int minTimetSecondsForStartNibble = 2;
    public int maxTimetSecondsForStartNibble = 8;

    public int CountBaitsInPurchase = 10;
    public Transform FishContainer;
    public Transform FloatContainer;
    public Transform LineContainer;
    public Transform HookContainer;

    public GameObject LakeBackground;

    private List<BobberData> Bobbers;
    private List<FishingRodData> FishinRods;
    private List<LakeData> Lakes;
    private List<FishlineData> Fishlines;
    private List<HookData> Hooks;
    private List<BaitData> Baits;

    [SerializeField] private List<FishData> _fishListForGame;
    public Material FishLineMaterial;

    [Space]
    [SerializeField] private List<Sprite> _pulseSprites;

    [SerializeField] private Image _pulseImage;
    public float pausePulse = 0.2f;
    public float pauseBetweenCyclesPulse = 0.2f;




    private Coroutine pulseCoroutine;

    private List<FishData> _fishList = new List<FishData>();
    private List<bool> _luckyStack = new List<bool>();
    private float _fishweight = 0f;
    private float _percentCatchedWeight = 0f;
    private float _sumCatched = 0f;
    private float _chanceValue;



    private List<string> _listAnimationForPlay = new List<string>();

    private SaveDataObject _saveDataObject;

    private CatchedFishInfo _catchedFishInfo;

    private float oldChanceValue;

    private void InitFishList()
    {
        _fishList.Clear();
        int LakeID = _saveDataObject.GetSaveData().LakeID;
        int BaitID = _saveDataObject.GetSaveData().BaitID;

        for (int indexFish = 0; indexFish < _fishListForGame.Count; indexFish++)
        {
            FishData currentFish = _fishListForGame[indexFish];
            for (int indexLake = 0; indexLake < currentFish.LakesLive.Count; indexLake++)
            {
                if (currentFish.LakesLive[indexLake].GetElementID() == LakeID)
                {
                    for (int indexBait = 0; indexBait < currentFish.Baites.Count; indexBait++)
                    {
                        if (currentFish.Baites[indexBait].GetElementID() == BaitID)
                        {
                            _fishList.Add(currentFish);
                        }
                    }
                }
            }
        }
    }

    public void SetChance(float value)
    {
        if (isNibbleState)
        {
            _chanceValue = value;
            VibrateOnNibble(value);
        }
    }

    private void VibrateOnNibble(float currentValue)
    {
        if(currentValue >= 9f)
        {
            if (oldChanceValue < 9f)
            {
                Vibro.PlayVibration();
            }
        }

        oldChanceValue = currentValue;
    }

    private void InitLuckyStack(bool isOnlyLuck = false)
    {
        InitFishList();

        int countTry = GetCountBaits();
        _luckyStack.Clear();

        for (int i = 0; i < countTry; i++)
        {
            _luckyStack.Add(true);
        }

        int countUnluckyTry = getCountUnluckyTry(countTry);

        if (countUnluckyTry > 0)
        {
            int countUpdatedTry = 0;

            while (countUpdatedTry < countUnluckyTry)
            {
                int randomPositionForUpdate = Random.Range(0, countTry);

                if (_luckyStack[randomPositionForUpdate])
                {
                    _luckyStack[randomPositionForUpdate] = false;
                    countUpdatedTry++;
                }
            }
        }

        if (isOnlyLuck)
        {
            int count;
            if(countTry > 10)
            {
                count = 10;
            } else
            {
                count = countTry;
            }

            for (int i = 0; i < count; i++)
            {
                _luckyStack[i] = true;
            }
        }

        /*
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _luckyStack.Count; i++)
        {
            if (_luckyStack[i])
            {
                sb.Append("1");
            }
            else
            {
                sb.Append("0");
            }
        }
        //Debug.Log("---------STACK = " + sb.ToString() + " ----------------------");
        */

        UpdateCountTryText();
        StartRandomNibble();
    }


    private int getCountUnluckyTry(int AllCountTry)
    {
        int result = 0;
        float maxRating = 36f;

        int BobberRating = GetRatingForSelectedShopItem(ShopItemType.BOBBER);
        int FishinRodRating = GetRatingForSelectedShopItem(ShopItemType.FISHINGROD);
        int LakeRating = GetRatingForSelectedShopItem(ShopItemType.LAKE);
        int FishlineRating = GetRatingForSelectedShopItem(ShopItemType.FISHLINE);
        int HookRating = GetRatingForSelectedShopItem(ShopItemType.HOOK);
        int BaitRating = GetRatingForSelectedShopItem(ShopItemType.BAIT);

        float sumRating = BobberRating + FishinRodRating + LakeRating + FishlineRating + HookRating + BaitRating;

        float percentLucky = ((sumRating / maxRating) * 100f);
        float percentUnLucky = 100f - percentLucky;

        result = (int)(((float)AllCountTry) / 100f * percentUnLucky);

        /*
        Debug.Log("----------------------------------" + "\n" +
        "Количество попыток: " + AllCountTry + "\n" +
        "Рейтинг поплавка: " + BobberRating + "\n" +
        "Рейтинг удочки: " + FishinRodRating + "\n" +
        "Рейтинг озера: " + LakeRating + "\n" +
        "Рейтинг лески: " + FishlineRating + "\n" +
        "Рейтинг крючка: " + HookRating + "\n" +
        "Рейтинг наживки: " + BaitRating + "\n" +
        "Общий рейтинг: " + sumRating + " Из 36" + "\n" +
        "% Удачи: " + percentLucky + "\n" +
        "% Неудачи: " + percentUnLucky + "\n" +
        "Количество неудачных попыток: " + result + "\n" +
        "-------------------------------------------");
        */

        return result;
    }


    private int GetRatingForSelectedShopItem(ShopItemType type)
    {
        switch (type)
        {
            case ShopItemType.BOBBER:
                for (int i = 0; i < Bobbers.Count; i++)
                {
                    if (Bobbers[i].GetElementID() == _saveDataObject.GetSaveData().BobberID)
                    {
                        return Bobbers[i].GetElementRating();
                    }
                }
                break;

            case ShopItemType.FISHINGROD:
                for (int i = 0; i < FishinRods.Count; i++)
                {
                    if (FishinRods[i].GetElementID() == _saveDataObject.GetSaveData().FishingrodID)
                    {
                        return FishinRods[i].GetElementRating();
                    }
                }
                break;

            case ShopItemType.LAKE:
                for (int i = 0; i < Lakes.Count; i++)
                {
                    if (Lakes[i].GetElementID() == _saveDataObject.GetSaveData().LakeID)
                    {
                        return Lakes[i].GetElementRating();
                    }
                }
                break;

            case ShopItemType.HOOK:
                for (int i = 0; i < Hooks.Count; i++)
                {
                    if (Hooks[i].GetElementID() == _saveDataObject.GetSaveData().HookID)
                    {
                        return Hooks[i].GetElementRating();
                    }
                }
                break;

            case ShopItemType.FISHLINE:
                for (int i = 0; i < Fishlines.Count; i++)
                {
                    if (Fishlines[i].GetElementID() == _saveDataObject.GetSaveData().FishLineID)
                    {
                        return Fishlines[i].GetElementRating();
                    }
                }
                break;

            case ShopItemType.BAIT:
                for (int i = 0; i < Baits.Count; i++)
                {
                    if (Baits[i].GetElementID() == _saveDataObject.GetSaveData().BaitID)
                    {
                        return Baits[i].GetElementRating();
                    }
                }
                break;
        }

        return 0;

    }




    private int GetCountBaits()
    {
        int result = 0;
        int selectedBaitID = _saveDataObject.GetSaveData().BaitID;

        for (int i = 0; i < _saveDataObject.GetSaveData().BuyedBaits.Count; i++)
        {
            List<int> currentElement = _saveDataObject.GetSaveData().BuyedBaits[i];
            if (currentElement[0] == selectedBaitID)
            {
                result = currentElement[1];
                break;
            }
        }

        return result;
    }

    
    void Start()
    {
        //DebugText.text = "";
        isIdleState = true;
        languageDictionary = FindObjectOfType<LanguageDictionary>();
        _saveDataObject = FindObjectOfType<SaveDataObject>();

        _catchedFishInfo = FindObjectOfType<CatchedFishInfo>();

        GamesObjectsDictionary gamesObjectsDictionary = FindObjectOfType<GamesObjectsDictionary>();
        Bobbers = gamesObjectsDictionary.GetBobbersList();
        FishinRods = gamesObjectsDictionary.GetFishinRodsList();
        Lakes = gamesObjectsDictionary.GetLakesList();
        Fishlines = gamesObjectsDictionary.GetFishlinesList();
        Hooks = gamesObjectsDictionary.GetHooksList();
        Baits = gamesObjectsDictionary.GetBaitsList();

        //InitLuckyStack(true);
        InitLuckyStack(true);

        InitFishingObjectsOnScene();
        StartRandomNibble();

        InitBackground();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }


    private void InitBackground()
    {
        if (_saveDataObject.GetSaveData().LakeID > 0)
        {
            LakeData lake = Lakes.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().LakeID);
            if (lake)
            {
                LakeBackground.GetComponent<Renderer>().material  = lake.GetMaterialForBackground();
            }
        }
    }




    





    private void InitFishingObjectsOnScene()
    {

        //Bobber
        if (_saveDataObject.GetSaveData().BobberID > 0)
        {
            if(_bobberObject)
            {
                Destroy(_bobberObject);
            }


            BobberData bobber = Bobbers.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().BobberID);
            if (bobber)
            {
                _bobberObject = Instantiate(bobber.GetPrefab(), FloatContainer.transform.position, Quaternion.identity);
                _bobberObject.transform.SetParent(FloatContainer);
            }
        }


        if (_bobberObject)
        {
            Transform FloatPivotPosition = _bobberObject.GetComponentInChildren<FloatPivotPoint>().gameObject.transform;
            if (FloatPivotPosition)
            {
                LineContainer.transform.position = FloatPivotPosition.position;
            }

        }

        /*
        //FishLine
        if (_saveDataObject.GetSaveData().FishLineID > 0)
        {
            FishlineData fishLine =  Fishlines.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().FishLineID);
            if(fishLine)
            {
                if (_fishLineObject)
                {
                    Destroy(_fishLineObject);
                }

                if (LineContainer)
                {
                    _fishLineObject = Instantiate(fishLine.GetPrefab(), LineContainer.transform.position,Quaternion.identity);
                    _fishLineObject.transform.SetParent(LineContainer);

                    Transform FishlinePivotPoint = _fishLineObject.GetComponentInChildren<FishLinePivot>().transform;
                    if (FishlinePivotPoint)
                    {
                        HookContainer.transform.position = FishlinePivotPoint.position;
                    }
                }
            }
        }

        //Hook
        if (_saveDataObject.GetSaveData().HookID > 0)
        {
            if (_hookObject)
            {
                Destroy(_hookObject);
            }
            HookData hook = Hooks.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().HookID);

            if (HookContainer)
            {
                if (hook)
                {
                    _hookObject = Instantiate(hook.GetPrefab(), HookContainer.transform.position, Quaternion.identity);
                    _hookObject.transform.SetParent(HookContainer);

                    Transform HookPivotPoint = _hookObject.GetComponentInChildren<HookPivot>().transform;
                    FishContainer.transform.position = HookPivotPoint.position;
                }
            }


            if (_hookObjectInfo)
            {
                Destroy(_hookObjectInfo);
            }

            if (hook)
            {
                _hookObjectInfo = Instantiate(hook.GetPrefab(), BaitContainer.transform.position, BaitContainer.transform.rotation, BaitContainer.transform);
            }

        }
        */


        if (_saveDataObject.GetSaveData().HookID > 0)
        {
            if (_hookObject)
            {
                Destroy(_hookObject);
            }
            HookData hook = Hooks.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().HookID);

            if (HookContainer)
            {
                if (hook)
                {
                    _hookObject = Instantiate(hook.GetPrefab(), HookContainer.transform.position, Quaternion.identity);
                    _hookObject.transform.SetParent(HookContainer);

                    Transform HookPivotPoint = _hookObject.GetComponentInChildren<HookPivot>().transform;
                    FishContainer.transform.position = HookPivotPoint.position;

                }
            }


            if (_hookObjectInfo)
            {
                Destroy(_hookObjectInfo);
            }

            if (hook)
            {
                _hookObjectInfo = Instantiate(hook.GetPrefab(), BaitContainer.transform.position, BaitContainer.transform.rotation, BaitContainer.transform);
            }


            if (_saveDataObject.GetSaveData().FishLineID > 0)
            {
                FishlineData fishLine = Fishlines.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().FishLineID);
                if (fishLine)
                {
                    if (_fishLineObject)
                    {
                        Destroy(_fishLineObject);
                    }
                   _fishLineObject = Instantiate(fishLine.GetPrefab(), HookContainer.transform.position, Quaternion.identity);
                   _fishLineObject.transform.SetParent(HookContainer);

                    FishLineMaterial.color = fishLine.GetFishlineColor();

                }
            }
        }

        InitBaitObject();
    }



    private void InitBaitObject()
    {

        if (_saveDataObject.GetSaveData().BaitID > 0)
        {
            if (_baitObject)
            {
                Destroy(_baitObject);
            }

            BaitData baitData = Baits.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().BaitID);
            if (baitData)
            {
                //_baitObject = Instantiate(baitData.GetPrefab(), FishContainer.transform.position, Quaternion.identity);
                _baitObject = Instantiate(baitData.GetPrefab(), _hookObject.transform.position, Quaternion.identity);

                if (_baitObject)
                {
                    _baitObject.transform.SetParent(FishContainer);
                }


                //Baitinfo
                if (_baitObjectInfo)
                {
                    Destroy(_baitObjectInfo);
                }

                //_baitObjectInfo = Instantiate(baitData.GetPrefab(), BaitContainer.transform.position, BaitContainer.transform.rotation, BaitContainer.transform);

                //Transform HookPivotPoint = _hookObjectInfo.GetComponentInChildren<HookPivot>().transform;
                //FishContainer.transform.position = HookPivotPoint.position;
                //_baitObjectInfo = Instantiate(baitData.GetPrefab(), HookPivotPoint.position, BaitContainer.transform.rotation, HookPivotPoint);

                _baitObjectInfo = Instantiate(baitData.GetPrefab(), _hookObjectInfo.transform.position, Quaternion.identity);


                //_baitObjectInfo BaitContainer

            }
        }
    }


    private void InitBaitObject_old_old()
    {

        //For boil we dont enable Baits
        /*
        if (_saveDataObject.GetSaveData().HookID == 6)
        {
            if (_baitObject)
            {
                Destroy(_baitObject);
            }
            return;
        }
        */


        if (_saveDataObject.GetSaveData().BaitID > 0)
        {
            if (_baitObject)
            {
                Destroy(_baitObject);
            }

            BaitData baitData = Baits.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().BaitID);
            if (baitData)
            {
                //_baitObject = Instantiate(baitData.GetPrefab(), FishContainer.transform.position, Quaternion.identity);
                _baitObject = Instantiate(baitData.GetPrefab(), _hookObject.transform.position, Quaternion.identity);
                FixedJoint fx = _baitObject.AddComponent<FixedJoint>();
                fx.connectedBody = _hookObject.GetComponent<Rigidbody>();



                //Baitinfo
                if (_baitObjectInfo)
                {
                    Destroy(_baitObjectInfo);
                }

                //_baitObjectInfo = Instantiate(baitData.GetPrefab(), BaitContainer.transform.position, BaitContainer.transform.rotation, BaitContainer.transform);

                //Transform HookPivotPoint = _hookObjectInfo.GetComponentInChildren<HookPivot>().transform;
                //FishContainer.transform.position = HookPivotPoint.position;
                //_baitObjectInfo = Instantiate(baitData.GetPrefab(), HookPivotPoint.position, BaitContainer.transform.rotation, HookPivotPoint);

                _baitObjectInfo = Instantiate(baitData.GetPrefab(), _hookObjectInfo.transform.position, Quaternion.identity);


                //_baitObjectInfo BaitContainer

            }
        }
    }


    private void Awake()
    {
        CheckAndHideHandAnimation();
    }

    //called from shakeDetecor when user is shake device
    public void ShakeDetected()
    {
        hideHandAnimation();
        if (isIdleState)
        {
            startCatchBoober();
        }
    }



    private void CheckAndHideHandAnimation()
    {
        if (!PlayerPrefs.HasKey("ShowHandAnimation"))
        {
            PlayerPrefs.SetInt("ShowHandAnimation", 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("ShowHandAnimation") == 0)
            {
                hideHandAnimation();
            }
        }
    }

    public void hideHandAnimation()
    {
        PlayerPrefs.SetInt("ShowHandAnimation", 0);
        if (handImage)
        {
            handImage.gameObject.SetActive(false);
        }
    }


    public void startCatchBoober()
    {

        if (_catchedFishObject)
        {
            Destroy(_catchedFishObject);
        }

        if (_luckyStack.Count == 0)
        {
            NoBaitsWarning();
            return;
        }


        if (isNibbleState)
        {
            //int catchNumber = Random.Range(0, 8);
            //EnableVisibleFish(catchNumber);
            isNibbleState = false;
            catchfish();
        }
        else
        {
            FishingMainAnimator.SetTrigger("CatchEmpty");
        }

        //FishingFloatAnimator.Play("EmptyLoop");
        FishingFloatAnimator.SetTrigger("StartCatch");

        isIdleState = false;
        isNibbleState = false;
        mainButton.GetComponent<Image>().sprite = nextSprite;
        cancelNibbleCoroutine();
    }


    private void NoBaitsWarning()
    {
        if (TryCountText.TryGetComponent<Animator>(out Animator animator))
        {
            animator.SetTrigger("StartAnimation");
        }
    }


    private void catchfish()
    {

        if (_luckyStack.Count > 0)
        {
            bool isCurrentTryLucky = _luckyStack[0];

            //chance check, if >= 9 then true
            if(_chanceValue >= 9f)
            {
                isCurrentTryLucky = true;
            }

            if (isCurrentTryLucky)
            {
                if(_baitObject)
                {
                    Destroy(_baitObject);
                }
                _luckyStack.RemoveAt(0);

                if(ChanceTry())
                {
                    SetFishParametersAndShow();
                } else
                {
                    if(_luckyStack.Count > 0)
                    {
                        _luckyStack.RemoveAt(0);
                    }
                    FishingMainAnimator.SetTrigger("CatchEmpty");
                }
                //FishingMainAnimator.SetTrigger("CatchFish");
            }
            else
            {
                _luckyStack.RemoveAt(0);
                FishingMainAnimator.SetTrigger("CatchEmpty");
            }
            ReduceCurrentBait();
            UpdateCountTryText();
        }
    }


    private bool ChanceTry()
    {

        Debug.Log("ChanceTry: value = " + _chanceValue);
        /*
         Chance range    percent true
         9 - 12           100%
         6 - 8            70% - 90%
         4 - 6            10% - 60%
         0 - 3            0%
         */
        bool result = true;

        if (_chanceValue >= 9f)
        {
            Debug.Log("Result = true");
            return true;
        }

        if(_chanceValue <= 3)
        {
            Debug.Log("Result = false");
            return false;
        }

        List<bool> _trysList = new List<bool>();
        int chanceIntValue = (int)_chanceValue;
        int luckyCount = 0;

        switch (chanceIntValue)
        {
            case 4:
                luckyCount = 1;
                break;
            case 5:
                luckyCount = 4;
                break;
            case 6:
                luckyCount = 7;
                break;
            case 7:
                luckyCount = 8;
                break;
            case 8:
                luckyCount = 9;
                break;
        }

        for (int i = 0; i < 10; i++)
        {
            if (i < luckyCount)
            {
                _trysList.Add(true);
            } else
            {
                _trysList.Add(false);
            }
        }

        result = _trysList[Random.Range(0, 10)];

        Debug.Log("Result = " + result);

        return result;
    }
    


    private void SetFishParametersAndShow()
    {
        if (_fishList.Count == 0 || FishContainer == null) return;

        int randomFishPosition = Random.Range(0, _fishList.Count);
        _catchedFish = _fishList[randomFishPosition];


        Transform HookPivotPoint = _hookObject.GetComponentInChildren<HookPivot>().transform;

        _catchedFishObject = Instantiate(_catchedFish.FishObject, FishContainer);
        _catchedFishObject.transform.SetParent(FishContainer);


        _catchedFishObject.name = "Fish";


        //Calculate weight fish
        FishingRodData currentFishingRod = GetSelecetdFishingRod();
        
        if (currentFishingRod)
        {      
            _percentCatchedWeight = currentFishingRod.GetPercentWeight();
        }

        _percentCatchedWeight = Random.Range(0.09f, _percentCatchedWeight + 0.01f);
        _fishweight = Mathf.Lerp(_catchedFish.MinWeight, _catchedFish.MaxWeight, _percentCatchedWeight);

        Vector3 originalScale = _catchedFish.FishObject.transform.localScale;
        Vector3 partScale = originalScale / 3f;
        Vector3 newScale = Vector3.Lerp(partScale, originalScale, _percentCatchedWeight);

        //Debug.Log("Scale original = " + _catchedFish.FishObject.transform.localScale);

        //Scale
        _catchedFishObject.transform.localScale = newScale;//_catchedFish.FishObject.transform.localScale * _percentCatchedWeight;

        //Debug.Log("_percentCatchedWeight = " + _percentCatchedWeight);
        //Debug.Log("Scale show = " + newScale);


        if (_fishweight > 0f && _catchedFish.Price > 0f)
        {
            _sumCatched = (_fishweight / 1000f) * _catchedFish.Price;
        }

        FishAnimator.enabled = true;

        //check if fishline can keep weight fish
        FishlineData currentFishline = GetSelecetdFishline();

        if (currentFishline.GetMaxWeight() >= _fishweight)
        {
            FishingMainAnimator.SetTrigger("CatchFish");

            _catchedFishInfo.SetValuesAndShow(languageDictionary.GetWord(_catchedFish.GetLanguage()), (_fishweight / 1000f), _sumCatched, (_fishweight / 1000f) * 8f);
            MenuCanvas.SetActive(false);
            //  _catchedFish.GetLanguage()


            //SaveCatchedFishToTrophey(_catchedFish.FishID, _fishweight, _sumCatched);
        } else
        {
            FishingMainAnimator.SetTrigger("BrokeFish");
        }
    }

    public void SaveFishClick()
    {
        Destroy(_catchedFishObject);
        SaveCatchedFishToTrophey(_catchedFish.FishID, _fishweight, _sumCatched);
        MenuCanvas.SetActive(true);
        PulseMainButton(true);
    }

    public void FreeFishClick()
    {
        Destroy(_catchedFishObject);
        MenuCanvas.SetActive(true);
        PulseMainButton(true);
    }


    private void SaveCatchedFishToTrophey(int FishID, float FishWeight, float SumCatch)
    {

        //Structue = [FishID][Wight][SUM]

        bool isFihDataNotUpdated = true;

        for (int indexTropheyInfo = 0; indexTropheyInfo < _saveDataObject.GetSaveData().TropheyInfo.Count; indexTropheyInfo++)
        {
            List<float> currentElement = _saveDataObject.GetSaveData().TropheyInfo[indexTropheyInfo];
            if ((currentElement[0]) == FishID)
            {
                currentElement[1] += FishWeight;
                currentElement[2] += SumCatch;
                isFihDataNotUpdated = false;
                break;
            }
        }

        if(isFihDataNotUpdated)
        {
            List<float> newTropheyInfo = new List<float>();
            newTropheyInfo.Add(FishID);
            newTropheyInfo.Add(FishWeight);
            newTropheyInfo.Add(SumCatch);
            _saveDataObject.GetSaveData().TropheyInfo.Add(newTropheyInfo);
        }
        _saveDataObject.saveGameData();
    }


    private FishingRodData GetSelecetdFishingRod()
    {
        int cuurrentFishingRodID = _saveDataObject.GetSaveData().FishingrodID;
        FishingRodData result = (FishingRodData) ScriptableObject.CreateInstance("FishingRodData");

        for (int i = 0; i < FishinRods.Count; i++)
        {
            if (FishinRods[i].GetElementID() == cuurrentFishingRodID)
            {
                result = FishinRods[i];
                break;
            }
        }

        return result;
    }


    private FishlineData GetSelecetdFishline()
    {
        int currentFishlinedID = _saveDataObject.GetSaveData().FishLineID;
        FishlineData result = (FishlineData) ScriptableObject.CreateInstance("FishlineData");
       

        for (int i = 0; i < Fishlines.Count; i++)
        {
            if (Fishlines[i].GetElementID() == currentFishlinedID)
            {
                result = Fishlines[i];
                break;
            }
        }

        return result;
    }


    private void ReduceCurrentBait()
    {
        int selectedBaitID = _saveDataObject.GetSaveData().BaitID;

        for (int i = 0; i < _saveDataObject.GetSaveData().BuyedBaits.Count; i++)
        {
            List<int> currentElement = _saveDataObject.GetSaveData().BuyedBaits[i];
            if (currentElement[0] == selectedBaitID)
            {
                currentElement[1] -= 1;
                _saveDataObject.saveGameData();
                break;
            }
        }
    }


    private void UpdateCountTryText()
    {
        if (TryCountText)
        {
            TryCountText.text = _luckyStack.Count.ToString();
        }
    }


    public void EffectBaitCreatedFisnished()
    {
        ReinitFishingMain();
        FishingMainAnimator.SetTrigger("ResetCatch");
        mainButton.GetComponent<Image>().sprite = fishermanSprite;
        FishAnimator.enabled = false;
        StartRandomNibble();

    }


    public void stopCatchBoober()
    {
        isIdleState = true;
        if(_baitObject == null)
        {
            EffectMakeBait.Play();
        } else
        {
            if(_baitObject.activeSelf == false)
            {
                EffectMakeBait.Play();
            } else
            {
                FishingFloatAnimator.SetTrigger("StopCatch");
                FishingMainAnimator.SetTrigger("ResetCatch");
                Invoke("ReinitFishingMain", 1);
                mainButton.GetComponent<Image>().sprite = fishermanSprite;
                FishAnimator.enabled = false;
                StartRandomNibble();
            }
        }
    }


    private void BaitCreateAnimation()
    {

    }

    private void ReinitFishingMain()
    {
        if (_catchedFishObject)
        {
            Destroy(_catchedFishObject);
        }

        InitBaitObject();
    }



    private int debugIndex = 0;

    public void MainButtonClick()
    {

        /*
        if (_catchedFishObject)
        {
            Destroy(_catchedFishObject);
        }

        _catchedFishObject = Instantiate(_fishListForGame[debugIndex].FishObject, FishContainer);

        if (debugIndex+1 < _fishListForGame.Count)
        {
            debugIndex++;
        }
        else
        {
            debugIndex = 0;
        }
        */


        PulseMainButton(false);

        if (Time.time > timestamp)
        {
            if (isIdleState)
            {
                startCatchBoober();
            }
            else
            {
                stopCatchBoober();
            }

            timestamp = Time.time + timeBetweenButtonClick;
        }

    }


    //Called from animation. Whne animation nibble is end
    public void StopNibble()
    {
        if (_listAnimationForPlay.Count > 0)
        {
            StartNibbleAnimation();
        }
        else
        {
              //после окончания поклевки - рандомно определеям, осталась наживка или нет
              int randomValue = Random.Range(0, 2);
              Debug.Log("=-=-=-=-=-=-=-= RandomValue = " + randomValue + " -=-=-=-=-=-=-=-=-=-=-=-=-=");
              if (randomValue == 1)
              {
                    //наживки нет, включаем пульсацию и отключаем поклев
                    PulseMainButton(true);
                    isIdleState = false;
                    isNibbleState = false;
                    cancelNibbleCoroutine();
              } else
              {
                isNibbleState = false;
                StartRandomNibble();
              }
        }
    }


    private void StartRandomNibble()
    {
        cancelNibbleCoroutine();
        InitListAnimations();
        if (_fishList.Count > 0)
        {
            int timeForStartNibble = Random.Range(minTimetSecondsForStartNibble, maxTimetSecondsForStartNibble);
            nibbleCoroutine = StartCoroutine(StartNibbleWithPause(timeForStartNibble));
        }
    }


    private void InitListAnimations()
    {
        _listAnimationForPlay.Clear();

        BaitData baitData = Baits.Find(item => item.GetElementID() == _saveDataObject.GetSaveData().BaitID);


        while(_listAnimationForPlay.Count < baitData.GetCountAnimations())
        {
            int numberAnimation = Random.Range(1, 5);
            string animationName = "";

            if (numberAnimation == 1)
            {
                animationName = "StartNibble";
            }

            if (numberAnimation == 2)
            {
                animationName = "StartNibble2";
            }

            if (numberAnimation == 3)
            {
                animationName = "StartNibble3";
            }

            if (numberAnimation == 4)
            {
                animationName = "StartNibble4";
            }

            if(!_listAnimationForPlay.Contains(animationName))
            {
                _listAnimationForPlay.Add(animationName);
            }

        }

        /*
        for (int i = 0; i < baitData.GetCountAnimations(); i++)
        {

            int numberAnimation = Random.Range(1, 5);

            if (numberAnimation == 1)
            {
                _listAnimationForPlay.Add("StartNibble");
            }

            if (numberAnimation == 2)
            {
                _listAnimationForPlay.Add("StartNibble2");
            }

            if (numberAnimation == 3)
            {
                _listAnimationForPlay.Add("StartNibble3");
            }

            if (numberAnimation == 4)
            {
                _listAnimationForPlay.Add("StartNibble4");
            }
        }
        */

    }

    private void cancelNibbleCoroutine()
    {
        if (nibbleCoroutine != null)
        {
            StopCoroutine(nibbleCoroutine);
        }
    }

    private IEnumerator StartNibbleWithPause(int PauseSeconds)
    {
        yield return new WaitForSeconds(PauseSeconds);
        isNibbleState = true;

        StartNibbleAnimation();
        //FishingFloatAnimator

    }



    private void StartNibbleAnimation()
    {
        /*
        int numberAnimation = Random.Range(1, 5);

        if (numberAnimation == 1)
        {
            FishingFloatAnimator.SetTrigger("StartNibble");
        }

        if (numberAnimation == 2)
        {
            FishingFloatAnimator.SetTrigger("StartNibble2");
        }

        if (numberAnimation == 3)
        {
            FishingFloatAnimator.SetTrigger("StartNibble3");
        }

        if (numberAnimation == 4)
        {
            FishingFloatAnimator.SetTrigger("StartNibble4");
        }
        */
        oldChanceValue = 0f;
        if (_listAnimationForPlay.Count > 0)
        {
            FishingFloatAnimator.SetTrigger(_listAnimationForPlay[0]);
            _listAnimationForPlay.RemoveAt(0);
        } 

    }



    public List<FishData> GetFishList()
    {
        return _fishListForGame;
    }

    private void OnApplicationQuit()
    {
        _saveDataObject.saveGameData();
    }


    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void OpenBaitsSelect()
    {
        var pref = _saveDataObject.GetSaveData();
        pref.returnToMainScene = true;
        pref.selectedKindInShop = SHOPKIND.BAIT;
        SceneManager.LoadScene("ByItemScene");

    }

    public void MakeSplashEffect(Transform point)
    {
        if (_catchedFishObject)
        {
            Instantiate(WaterSplashEffect, point.position, point.rotation);
        }
    }


    private void PulseMainButton(bool isPlay)
    {
        if (isPlay)
        {
            _pulseImage.gameObject.SetActive(true);
            pulseCoroutine = StartCoroutine(StartPulseCoroutine());
        } else
        {
            if(pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
            }
            _pulseImage.gameObject.SetActive(false);
        }
    }


    private IEnumerator StartPulseCoroutine()
    {
        /*        
        int currentIndex = -1;
        bool forward = true;

        while (true)
        {

            if(currentIndex < 0)
            {
                _pulseImage.gameObject.SetActive(false);
            } else
            {
                _pulseImage.gameObject.SetActive(true);
                _pulseImage.sprite = _pulseSprites[currentIndex];
            }


            yield return new WaitForSeconds(pausePulse);

            if (forward)
            {
                if(currentIndex <= 2)
                {
                    currentIndex++;
                } else
                {
                    forward = false;
                }
            } else
            {
                if(currentIndex >= 0)
                {
                    currentIndex--;
                    if(currentIndex < 0)
                    {
                        forward = true;
                    }
                }
            }
        }
        */
        while (true)
        {
            _pulseImage.gameObject.SetActive(true);
            for (int i = 0; i < _pulseSprites.Count; i++)
            {
                _pulseImage.sprite = _pulseSprites[i];
                yield return new WaitForSeconds(pausePulse);
            }
            _pulseImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(pauseBetweenCyclesPulse);
        }


    }


}


