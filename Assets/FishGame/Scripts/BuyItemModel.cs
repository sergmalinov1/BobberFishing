using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuyItemModel : MonoBehaviour
{
    private SaveDataObject saveDataObject;
    private SaveData preferences;
    private GamesObjectsDictionary _gamesObjectsDictionary;
    private LanguageDictionary _languageDictionary;

    private List<BobberData> Bobbers;
    private List<FishingRodData> FishinRods;
    private List<LakeData> Lakes;
    private List<FishlineData> Fishlines;
    private List<HookData> Hooks;
    private List<BaitData> Baits;

    public TMP_Text ItemName;
    public TMP_Text ItemPrice;
    public GameObject ButtonPrevious;
    public GameObject ButtonNext;
    public Transform CameraPositoin;

    public Image RatingImage;
    public Sprite Rating1;
    public Sprite Rating2;
    public Sprite Rating3;
    public Sprite Rating4;
    public Sprite Rating5;
    public Sprite Rating6;

    public Transform SpawnTransform;
    private GameObject currentPrefab;

    private int IndexCurrentElement;
    private ShopItemInterface SavedItem;
    private int MaxIndex;

    public GameObject ShopIconLock;
    public GameObject ShopIconApply;
    public GameObject ShopButtonApply;
    public GameObject ShopButtonBuy;

    public int CountBaitsInPurchase = 10;
    public GameObject NoMoneyElement;

    public TMPro.TMP_Text textScoreValue;
    public TMPro.TMP_Text textBaitsAvailable;
    public Image PriceImage;
    public Image BaitsAvailable;

    private int _baitsAvailable;

    public Image LakePhoto;
    public GameObject JoystickControl;

    [Space]
    public Color SelectedHeadColor;
    public Color DefaultHeadColor;
    public Color DisbaledHeadColor;

    void Start()
    {
      
        saveDataObject = FindObjectOfType<SaveDataObject>();
        if (saveDataObject)
        {
            preferences = saveDataObject.GetSaveData();
        }
        else
        {
            return;
        }

       _languageDictionary = FindObjectOfType<LanguageDictionary>();

        ButtonPrevious.SetActive(false);
        ButtonNext.SetActive(false);

        _gamesObjectsDictionary = FindObjectOfType<GamesObjectsDictionary>();
        Bobbers = _gamesObjectsDictionary.GetBobbersList();
        FishinRods = _gamesObjectsDictionary.GetFishinRodsList();
        Lakes = _gamesObjectsDictionary.GetLakesList();
        Fishlines = _gamesObjectsDictionary.GetFishlinesList();
        Hooks = _gamesObjectsDictionary.GetHooksList();
        Baits = _gamesObjectsDictionary.GetBaitsList();

        if (preferences.selectedKindInShop == SHOPKIND.LAKE)
        {
            SavedItem = GetShopItemByID(preferences.LakeID, ShopItemType.LAKE);
            LakePhoto.gameObject.SetActive(true);
            JoystickControl.SetActive(false);
            MaxIndex = Lakes.Count;
        }

        if (preferences.selectedKindInShop == SHOPKIND.BOBBER)
        {
            SavedItem = GetShopItemByID(preferences.BobberID, ShopItemType.BOBBER);
            MaxIndex = Bobbers.Count;
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHLINE)
        {
            SavedItem = GetShopItemByID(preferences.FishLineID, ShopItemType.FISHLINE);
            MaxIndex = Fishlines.Count;
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHROD)
        {
            SavedItem = GetShopItemByID(preferences.FishingrodID, ShopItemType.FISHINGROD);
            MaxIndex = FishinRods.Count;
        }

        if (preferences.selectedKindInShop == SHOPKIND.HOOK)
        {
            SavedItem = GetShopItemByID(preferences.HookID, ShopItemType.HOOK);
            MaxIndex = Hooks.Count;
        }

        if (preferences.selectedKindInShop == SHOPKIND.BAIT)
        {
            SavedItem = GetShopItemByID(preferences.BaitID, ShopItemType.BAIT);
            MaxIndex = Baits.Count;
            UpdateBuyedBait(SavedItem, preferences.ItemBaits, preferences.BuyedBaits);
        }

        RecalcCurrentElement();
    }


    private void UpdateBuyedBait(ShopItemInterface element, List<int> BuyedList, List<List<int>> BaitsQty)
    {
        textBaitsAvailable.text = "0";
        _baitsAvailable = 0;

        PriceImage.gameObject.SetActive(true);
        BaitsAvailable.gameObject.SetActive(false);

        for (int indexBuyed = 0; indexBuyed < BuyedList.Count; indexBuyed++)
        {
            if (BuyedList[indexBuyed] == element.GetElementID())
            {
                for (int indexBaits = 0; indexBaits < BaitsQty.Count; indexBaits++)
                {
                    List<int> currentElement = BaitsQty[indexBaits];
                    if (currentElement[0] == BuyedList[indexBuyed])
                    {
                        textBaitsAvailable.text = currentElement[1].ToString();
                        _baitsAvailable = currentElement[1];
                        if(_baitsAvailable > 0)
                        {
                            PriceImage.gameObject.SetActive(false);
                            BaitsAvailable.gameObject.SetActive(true);
                        }
                        break;
                    }
                }

            }
        }
    }



    private void RecalcCurrentElement()
    {
        updateScore();

        if (IsCurrentElementSelected())
        {
            Camera.main.backgroundColor = SelectedHeadColor;
        } else
        {
            if (IsElementBuyed(SavedItem.GetElementID()))
            {
                Camera.main.backgroundColor = DefaultHeadColor;
            }
            else
            {
                if (isPreviousElementBuyed())
                {
                    Camera.main.backgroundColor = DefaultHeadColor;
                }
                else
                {
                    Camera.main.backgroundColor = DisbaledHeadColor;
                }
            }
        }


        if (SavedItem.GetLanguage() == null)
        {
            ItemName.text = SavedItem.GetElementName() + SavedItem.GetMemo();
        } else {
            ItemName.text = _languageDictionary.GetWord(SavedItem.GetLanguage(), SavedItem.GetMemo());
        }

        if(currentPrefab)
        {
            Destroy(currentPrefab);
        }

        if (preferences.selectedKindInShop == SHOPKIND.LAKE)
        {
            LakePhoto.sprite = SavedItem.GetElementSprite();
            LakePhoto.fillCenter = true;
        }
        else
        {
            if (SavedItem.GetPrefabForShop())
            {
                SpawnTransform.rotation = Quaternion.identity;
                SpawnTransform.localScale = Vector3.one;
                CameraPositoin.position = new Vector3(0f, 0f, -7.22f);
                currentPrefab = Instantiate(SavedItem.GetPrefabForShop(), SpawnTransform.position, SpawnTransform.rotation, SpawnTransform);
                currentPrefab.transform.localScale = Vector3.one;

                if (preferences.selectedKindInShop == SHOPKIND.FISHROD)
                {
                    Vector3 newPos = currentPrefab.transform.position;
                    newPos += new Vector3(0f,-2f,0f);
                    currentPrefab.transform.Translate(newPos);
                }

                if (preferences.selectedKindInShop == SHOPKIND.BAIT)
                {
                    currentPrefab.transform.localScale = Vector3.one * ((BaitData)SavedItem).GetScaleForShopItem();
                }

                if (preferences.selectedKindInShop == SHOPKIND.HOOK)
                {
                    currentPrefab.transform.localScale = Vector3.one * ((HookData)SavedItem).GetScaleForShopItem();
                }
            }
        }

        ItemPrice.text = SavedItem.GetElementPrice().ToString();
        ShowRating(SavedItem.GetElementRating());
        SetControlButtons();
        SetShopButton();
    }


    private void ShowRating(int ratingValue)
    {
        RatingImage.gameObject.SetActive(true);
        if (ratingValue == 1)
        {
            RatingImage.sprite = Rating1;
        } else if (ratingValue == 2)
        {
            RatingImage.sprite = Rating2;
        } else if (ratingValue == 3)
        {
            RatingImage.sprite = Rating3;
        } else if (ratingValue == 4)
        {
            RatingImage.sprite = Rating4;
        } else if (ratingValue == 5)
        {
            RatingImage.sprite = Rating5;
        } else if (ratingValue == 6)
        {
            RatingImage.sprite = Rating6;
        } else
        {
            RatingImage.gameObject.SetActive(false);
        }
    }

    private void SetControlButtons()
    {
        bool enableNextButton = false;
        if(IndexCurrentElement == 0)
        {
            ButtonPrevious.SetActive(false);
        } else
        {
            ButtonPrevious.SetActive(true);
        }


        if(IndexCurrentElement < (MaxIndex - 1))
        {
            enableNextButton = true;
        }

        ButtonNext.SetActive(enableNextButton);
    }


    public void BackClick()
    {
        if(preferences.returnToMainScene)
        {
            SceneManager.LoadScene("mainScene");
        } else
        {
            SceneManager.LoadScene("ShopScene");
        }
    }


    private ShopItemInterface GetShopItemByID(int id, ShopItemType type)
    {
        switch (type)
        {
            case ShopItemType.BOBBER:
                for (int i = 0; i < Bobbers.Count; i++)
                {
                    if (Bobbers[i].GetElementID() == id)
                    {
                        IndexCurrentElement = i;
                        return Bobbers[i];
                    }
                }
                break;

            case ShopItemType.FISHINGROD:
                for (int i = 0; i < FishinRods.Count; i++)
                {
                    if (FishinRods[i].GetElementID() == id)
                    {
                        IndexCurrentElement = i;
                        return FishinRods[i];
                    }
                }
                break;

            case ShopItemType.LAKE:
                for (int i = 0; i < Lakes.Count; i++)
                {
                    if (Lakes[i].GetElementID() == id)
                    {
                        IndexCurrentElement = i;
                        return Lakes[i];
                    }
                }
                break;

            case ShopItemType.HOOK:
                for (int i = 0; i < Hooks.Count; i++)
                {
                    if (Hooks[i].GetElementID() == id)
                    {
                        IndexCurrentElement = i;
                        return Hooks[i];
                    }
                }
                break;

            case ShopItemType.FISHLINE:
                for (int i = 0; i < Fishlines.Count; i++)
                {
                    if (Fishlines[i].GetElementID() == id)
                    {
                        IndexCurrentElement = i;
                        return Fishlines[i];
                    }
                }
                break;

            case ShopItemType.BAIT:
                for (int i = 0; i < Baits.Count; i++)
                {
                    if (Baits[i].GetElementID() == id)
                    {
                        IndexCurrentElement = i;
                        return Baits[i];
                    }
                }
                break;
        }

        return null;

    }

    public void OnButtonNextClick()
    {
        if(IndexCurrentElement < MaxIndex-1)
        {
            IndexCurrentElement++;
            UpdateItem();
        }
    }

    public void OnButtonPreviousClick()
    {
        if(IndexCurrentElement > 0)
        {
            IndexCurrentElement--;
        }
        UpdateItem();
    }

    private void UpdateItem()
    {
        if (preferences.selectedKindInShop == SHOPKIND.LAKE)
        {
             SavedItem = GetShopItemByID(Lakes[IndexCurrentElement].GetElementID(), ShopItemType.LAKE);
        }

        if (preferences.selectedKindInShop == SHOPKIND.BOBBER)
        {
            SavedItem = GetShopItemByID(Bobbers[IndexCurrentElement].GetElementID(), ShopItemType.BOBBER);
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHLINE)
        {
            SavedItem = GetShopItemByID(Fishlines[IndexCurrentElement].GetElementID(), ShopItemType.FISHLINE);
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHROD)
        {
            SavedItem = GetShopItemByID(FishinRods[IndexCurrentElement].GetElementID(), ShopItemType.FISHINGROD);
        }

        if (preferences.selectedKindInShop == SHOPKIND.HOOK)
        {
            SavedItem = GetShopItemByID(Hooks[IndexCurrentElement].GetElementID(), ShopItemType.HOOK);
        }

        if (preferences.selectedKindInShop == SHOPKIND.BAIT)
        {
            SavedItem = GetShopItemByID(Baits[IndexCurrentElement].GetElementID(), ShopItemType.BAIT);
            UpdateBuyedBait(SavedItem, preferences.ItemBaits, preferences.BuyedBaits);
        }

        RecalcCurrentElement();
    }


    private void SetShopButton()
    {
        ShopIconLock.SetActive(false);
        ShopIconApply.SetActive(false);
        ShopButtonApply.SetActive(false);
        ShopButtonBuy.SetActive(false);

        if (preferences.selectedKindInShop == SHOPKIND.BAIT)
        {
            if (IsCurrentElementSelected())
            {
                if(_baitsAvailable > 0)
                {
                    ShopIconApply.SetActive(true);
                } else
                {
                    ShopButtonBuy.SetActive(true);
                }
            }
            else
            {
                if (IsElementBuyed(SavedItem.GetElementID()))
                {
                    ShopButtonApply.SetActive(true);
                }
                else
                {
                    if (isPreviousElementBuyed())
                    {
                        ShopButtonBuy.SetActive(true);
                    }
                    else
                    {
                        ShopIconLock.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (IsCurrentElementSelected())
            {
                ShopIconApply.SetActive(true);
            }
            else
            {
                if (IsElementBuyed(SavedItem.GetElementID()))
                {
                    ShopButtonApply.SetActive(true);
                }
                else
                {
                    if (isPreviousElementBuyed())
                    {
                        ShopButtonBuy.SetActive(true);
                    }
                    else
                    {
                        ShopIconLock.SetActive(true);
                    }
                }
            }
        }
    }



    private bool IsCurrentElementSelected()
    {
        bool result = false;

        if (preferences.selectedKindInShop == SHOPKIND.LAKE)
        {
            result = (SavedItem.GetElementID() == preferences.LakeID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.BOBBER)
        {
            result = (SavedItem.GetElementID() == preferences.BobberID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHLINE)
        {
            result = (SavedItem.GetElementID() == preferences.FishLineID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHROD)
        {
            result = (SavedItem.GetElementID() == preferences.FishingrodID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.HOOK)
        {
            result = (SavedItem.GetElementID() == preferences.HookID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.BAIT)
        {
            result = (SavedItem.GetElementID() == preferences.BaitID);
        }
        return result;
    }


    private bool IsElementBuyed(int elementID)
    {
        bool result = false;

        if (preferences.selectedKindInShop == SHOPKIND.LAKE)
        {
            result = preferences.ItemLakes.Contains(elementID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.BOBBER)
        {
            result = preferences.ItemBobbers.Contains(elementID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHLINE)
        {
            result = preferences.ItemFishlines.Contains(elementID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.FISHROD)
        {
            result = preferences.ItemFishingRods.Contains(elementID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.HOOK)
        {
            result = preferences.ItemHooks.Contains(elementID);
        }

        if (preferences.selectedKindInShop == SHOPKIND.BAIT)
        {
            result = preferences.ItemBaits.Contains(elementID);
        }
        return result;
    }

    private bool isPreviousElementBuyed()
    {
        bool result = false;

        if(IndexCurrentElement > 0)
        {
            if (preferences.selectedKindInShop == SHOPKIND.LAKE)
            {
                result = IsElementBuyed(Lakes[IndexCurrentElement - 1].GetElementID());
            }

            if (preferences.selectedKindInShop == SHOPKIND.BOBBER)
            {
                result = IsElementBuyed(Bobbers[IndexCurrentElement - 1].GetElementID());
            }

            if (preferences.selectedKindInShop == SHOPKIND.FISHLINE)
            {
                result = IsElementBuyed(Fishlines[IndexCurrentElement - 1].GetElementID());
            }

            if (preferences.selectedKindInShop == SHOPKIND.FISHROD)
            {
                result = IsElementBuyed(FishinRods[IndexCurrentElement - 1].GetElementID());
            }

            if (preferences.selectedKindInShop == SHOPKIND.HOOK)
            {
                result = IsElementBuyed(Hooks[IndexCurrentElement - 1].GetElementID());
            }

            if (preferences.selectedKindInShop == SHOPKIND.BAIT)
            {
                result = IsElementBuyed(Baits[IndexCurrentElement - 1].GetElementID());
            }
        }

        return result;
    }


    public void ApplyCurrentItemClick()
    {
        SaveSelectedShopItem(SavedItem, false);
        UpdateItem();
    }


    private void SaveSelectedShopItem(ShopItemInterface element, bool isBuy)
    {

        switch (element.GetElementType())
        {
            case ShopItemType.BOBBER:
                preferences.BobberID = element.GetElementID();
                if (isBuy)
                {
                    preferences.ItemBobbers.Add(element.GetElementID());
                }
                break;

            case ShopItemType.FISHINGROD:
                preferences.FishingrodID = element.GetElementID();
                if (isBuy)
                {
                    preferences.ItemFishingRods.Add(element.GetElementID());
                }
                break;

            case ShopItemType.LAKE:
                preferences.LakeID = element.GetElementID();
                if (isBuy)
                {
                    preferences.ItemLakes.Add(element.GetElementID());
                }

                break;

            case ShopItemType.HOOK:
                preferences.HookID = element.GetElementID();
                if (isBuy)
                {
                    preferences.ItemHooks.Add(element.GetElementID());
                }
                break;

            case ShopItemType.FISHLINE:
                preferences.FishLineID = element.GetElementID();
                if (isBuy)
                {
                    preferences.ItemFishlines.Add(element.GetElementID());
                }
                break;

            case ShopItemType.BAIT:
                preferences.BaitID = element.GetElementID();
                if (isBuy)
                {
                    preferences.ItemBaits.Add(element.GetElementID());
                    AddBuyedBaitToStore(element.GetElementID());
                }
                break;
        }

        saveDataObject.saveGameData();
    }

    private void AddBuyedBaitToStore(int baitID)
    {
        bool baitExists = false;

        for (int i = 0; i < preferences.BuyedBaits.Count; i++)
        {
            List<int> currentElement = preferences.BuyedBaits[i];
            if (currentElement[0] == baitID)
            {
                currentElement[1] = currentElement[1] + CountBaitsInPurchase;
                baitExists = true;
            }
        }

        if (!baitExists)
        {
            List<int> tmpElement = new List<int>();
            tmpElement.Add(baitID);
            tmpElement.Add(CountBaitsInPurchase);
            preferences.BuyedBaits.Add(tmpElement);
        }

    }

    public void BuyElement()
    {
        if (SavedItem.GetElementPrice() <= preferences.m_Score)
        {
            preferences.m_Score -= SavedItem.GetElementPrice();
            SaveSelectedShopItem(SavedItem, true);
            UpdateItem();
        }
        else
        {
            ShowNoMoney();
        }
    }

    public void ShowNoMoney()
    {
        if (NoMoneyElement != null)
        {
            NoMoneyElement.SetActive(true);
        }
    }

    private void updateScore()
    {
        if (textScoreValue != null)
        {
            if (textScoreValue.enabled)
            {
                textScoreValue.text = preferences.m_Score.ToString();
            }
        }
    }

}
