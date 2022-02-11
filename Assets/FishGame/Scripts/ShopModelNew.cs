using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SHOPKIND
{
    LAKE,
    BOBBER,
    FISHLINE,
    FISHROD,
    HOOK,
    BAIT
}

public class ShopModelNew : MonoBehaviour
{

    private List<BobberData> Bobbers;
    private List<FishingRodData> FishinRods;
    private List<LakeData> Lakes;
    private List<FishlineData> Fishlines;
    private List<HookData> Hooks;
    private List<BaitData> Baits;

    private GamesObjectsDictionary _gamesObjectsDictionary;

    public ButtonCard BobberSelectedCard;
    public ButtonCard FishingRodSelectedCard;
    public ButtonCard LakeSelectedCard;
    public ButtonCard HookSelectedCard;
    public ButtonCard FishLineSelectedCard;
    public ButtonCard BaitSelectedCard;

    public GameObject NoMoneyElement;
    public TMPro.TMP_Text textScoreValue;

    private SaveData preferences;

    private List<GameObject> BobbersList = new List<GameObject>();
    private List<GameObject> FishingRodList = new List<GameObject>();
    private List<GameObject> LakeList = new List<GameObject>();
    private List<GameObject> HookList = new List<GameObject>();
    private List<GameObject> FishLineList = new List<GameObject>();
    private List<GameObject> BaitList = new List<GameObject>();

    private SaveDataObject saveDataObject;

    private ShopItemInterface _shopElementForBuy;

    public Transform ScrollViewContentInPopUp;
    public GameObject PopUpSelectItem;
    public GameObject PopUpShopView;
    public PopUpShop _popUPShopModel;
    public int CountBaitsInPurchase = 10;

    private Donate _donate;

    private List<GameObject> ObjectsOnScroll = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        saveDataObject = FindObjectOfType<SaveDataObject>();
        if (saveDataObject)
        {
            preferences = saveDataObject.GetSaveData();
        } else
        {
            return;
        }

        _gamesObjectsDictionary = FindObjectOfType<GamesObjectsDictionary>();
        Bobbers = _gamesObjectsDictionary.GetBobbersList();
        FishinRods = _gamesObjectsDictionary.GetFishinRodsList();
        Lakes = _gamesObjectsDictionary.GetLakesList();
        Fishlines = _gamesObjectsDictionary.GetFishlinesList();
        Hooks = _gamesObjectsDictionary.GetHooksList();
        Baits = _gamesObjectsDictionary.GetBaitsList();


        _donate = FindObjectOfType<Donate>();
        _donate.OnDonateSuccessfull.AddListener(updateScore);

        UpdateAllBueydItems();

        ShopItemInterface selectedBobber = GetShopItemByID(preferences.BobberID, ShopItemType.BOBBER);
        ShopItemInterface selectedFishrod = GetShopItemByID(preferences.FishingrodID, ShopItemType.FISHINGROD);
        ShopItemInterface selectedLake = GetShopItemByID(preferences.LakeID, ShopItemType.LAKE);
        ShopItemInterface selectedHook = GetShopItemByID(preferences.HookID, ShopItemType.HOOK);
        ShopItemInterface selectedFishLine = GetShopItemByID(preferences.FishLineID, ShopItemType.FISHLINE);
        ShopItemInterface selectedBait = GetShopItemByID(preferences.BaitID, ShopItemType.BAIT);

        if (selectedBobber != null) SetSelectedElement(selectedBobber);
        if (selectedFishrod != null) SetSelectedElement(selectedFishrod);
        if (selectedLake != null) SetSelectedElement(selectedLake);
        if (selectedHook != null) SetSelectedElement(selectedHook);
        if (selectedFishLine != null) SetSelectedElement(selectedFishLine);
        if (selectedBait != null) SetSelectedElement(selectedBait);
        RefreshData();
    }


    public void RefreshData()
    {
        UpdateBuyedBaits(BaitList, preferences.ItemBaits, preferences.BuyedBaits);
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
                        return Bobbers[i];
                    }
                }
                break;

            case ShopItemType.FISHINGROD:
                for (int i = 0; i < FishinRods.Count; i++)
                {
                    if (FishinRods[i].GetElementID() == id)
                    {
                        return FishinRods[i];
                    }
                }
                break;

            case ShopItemType.LAKE:
                for (int i = 0; i < Lakes.Count; i++)
                {
                    if (Lakes[i].GetElementID() == id)
                    {
                        return Lakes[i];
                    }
                }
                break;

            case ShopItemType.HOOK:
                for (int i = 0; i < Hooks.Count; i++)
                {
                    if (Hooks[i].GetElementID() == id)
                    {
                        return Hooks[i];
                    }
                }
                break;

            case ShopItemType.FISHLINE:
                for (int i = 0; i < Fishlines.Count; i++)
                {
                    if (Fishlines[i].GetElementID() == id)
                    {
                        return Fishlines[i];
                    }
                }
                break;

            case ShopItemType.BAIT:
                for (int i = 0; i < Baits.Count; i++)
                {
                    if (Baits[i].GetElementID() == id)
                    {
                        return Baits[i];
                    }
                }
                break;
        }

        return null;

    }



    public void UpdateAllBueydItems()
    {
        /*
        UpdateBueydItems(BobbersList, preferences.ItemBobbers);
        UpdateBueydItems(FishingRodList, preferences.ItemFishingRods);
        UpdateBueydItems(LakeList, preferences.ItemLakes);
        UpdateBueydItems(HookList, preferences.ItemHooks);
        UpdateBueydItems(FishLineList, preferences.ItemFishlines);
        UpdateBuyedBaits(BaitList, preferences.ItemBaits, preferences.BuyedBaits);
        */
        updateScore();
    }


    private void UpdateBueydItems(List<GameObject> ListItems, List<int> BuyedList)
    {
        for (int indexItem = 0; indexItem < ListItems.Count; indexItem++)
        {
            if (ListItems[indexItem])
            {
                CardScript card = ListItems[indexItem].GetComponent<CardScript>();
                card.UpdateLanguageText();

                for (int indexBuyed = 0; indexBuyed < BuyedList.Count; indexBuyed++)
                {
                    if (BuyedList[indexBuyed] == card.GetID())
                    {
                        card.SetBuyed(true);
                    }
                }
            }
        }
    }


    private void UpdateBuyedBaits(List<GameObject> ListItems, List<int> BuyedList, List<List<int>> BaitsQty)
    {
        for (int indexItem = 0; indexItem < ListItems.Count; indexItem++)
        {
            CardScript card = ListItems[indexItem].GetComponent<CardScript>();
            card.UpdateLanguageText();
            for (int indexBuyed = 0; indexBuyed < BuyedList.Count; indexBuyed++)
            {
                if (BuyedList[indexBuyed] == card.GetID())
                {
                    card.SetBuyed(true);
                    for (int indexBaits = 0; indexBaits < BaitsQty.Count; indexBaits++)
                    {
                        List<int> currentElement = BaitsQty[indexBaits];
                        if (currentElement[0] == BuyedList[indexBuyed])
                        {
                            card.SetQtyAvailable(currentElement[1]);
                            break;
                        }
                    }

                }
            }
        }
    }



    private void UpdateBuyedBait(ButtonCard card, ShopItemInterface element, List<int> BuyedList, List<List<int>> BaitsQty)
    {
        card.SetQtyAvailable(0);
        for (int indexBuyed = 0; indexBuyed < BuyedList.Count; indexBuyed++)
        {
            if (BuyedList[indexBuyed] == element.GetElementID())
            {
                for (int indexBaits = 0; indexBaits < BaitsQty.Count; indexBaits++)
                {
                    List<int> currentElement = BaitsQty[indexBaits];
                    if (currentElement[0] == BuyedList[indexBuyed])
                    {
                        card.SetQtyAvailable(currentElement[1]);
                        break;
                    }
                }

            }
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


    // Update is called once per frame
    void Update()
    {
        //updateScore();
    }


    public void SetSelectedElement(ShopItemInterface element)
    {
        switch (element.GetElementType())
        {
            case ShopItemType.BOBBER:
                UpdateSelectedCard(BobberSelectedCard, element);
                break;

            case ShopItemType.FISHINGROD:
                UpdateSelectedCard(FishingRodSelectedCard, element);
                break;

            case ShopItemType.LAKE:
                UpdateSelectedCard(LakeSelectedCard, element);
                break;

            case ShopItemType.HOOK:
                UpdateSelectedCard(HookSelectedCard, element);
                break;

            case ShopItemType.FISHLINE:
                UpdateSelectedCard(FishLineSelectedCard, element);
                break;

            case ShopItemType.BAIT:
                UpdateSelectedCard(BaitSelectedCard, element);
                UpdateBuyedBait(BaitSelectedCard,element, preferences.ItemBaits, preferences.BuyedBaits);
                break;
        }
    }

    private void UpdateSelectedCard(ButtonCard Card, ShopItemInterface element)
    {
        Card.Init(element);
        //Card.GetComponent<SelectedCardScript>().Init(element);
    }


    public void ShowNoMoney()
    {
        if (NoMoneyElement != null)
        {
            NoMoneyElement.SetActive(true);
            //Invoke(nameof(HideNomMoney), 2f);
        }
    }

    public void HideNomMoney()
    {
        NoMoneyElement.SetActive(false);
    }


    public void ButtonBackOnClick()
    {
        SceneManager.LoadScene("mainScene");
    }



    public void BuyElement(ShopItemInterface itemData)
    {
        _shopElementForBuy = itemData;
        if (itemData.GetElementPrice() <= preferences.m_Score)
        {
            PopUpSelectItem.SetActive(false);
            PopUpShopView.gameObject.SetActive(true);
            if (_popUPShopModel)
            {
                _popUPShopModel.Init(itemData);
            }
        }
        else
        {
            ShowNoMoney();
        }
    }

    public void ActivateElement(ShopItemInterface element)
    {
        SetSelectedElement(element);
        SaveSelectedShopItem(element, false);
        PopUpSelectItem.SetActive(false);
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


    public void PoupBuyClick()
    {
        if (_shopElementForBuy != null)
        {
            SaveSelectedShopItem(_shopElementForBuy, true);
            preferences.m_Score -= _shopElementForBuy.GetElementPrice();
            UpdateAllBueydItems();
            SetSelectedElement(_shopElementForBuy);
            saveDataObject.saveGameData();
        }
        ClosePopUpShop();
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

    public void ClosePopUpShop()
    {
        PopUpShopView.gameObject.SetActive(false);
    }

    public void DonateClick()
    {
        _donate.StartDonate();
    }

    public void FishFreeClick()
    {
            
    }

    public void FishGetClick()
    {

    }




    public void ShowPopupBobber()
    {
        ShowKindForSelect(SHOPKIND.BOBBER);
    }

    public void ShowPopupLake()
    {
        ShowKindForSelect(SHOPKIND.LAKE);
    }


    public void ShowPopupFishLine()
    {
        ShowKindForSelect(SHOPKIND.FISHLINE);
    }


    public void ShowPopupFishrod()
    {
        ShowKindForSelect(SHOPKIND.FISHROD);
    }


    public void ShowPopupHook()
    {
        ShowKindForSelect(SHOPKIND.HOOK);
    }


    public void ShowPopupBait()
    {
        ShowKindForSelect(SHOPKIND.BAIT);
    }


    private void ShowKindForSelect(SHOPKIND shopKind)
    {

        //ScrollViewContentInPopUp;
        //public GameObject PopUpSelectItem;

        /*
        foreach (var b in BobbersList)
        {
            Destroy(b);
        }
        foreach (var f in FishingRodList)
        {
            Destroy(f);
        }
        foreach (var l in LakeList)
        {
            Destroy(l);
        }
        foreach (var h in HookList)
        {
            Destroy(h);
        }
        foreach (var fl in FishLineList)
        {
            Destroy(fl);
        }
        foreach (var bt in BaitList)
        {
            Destroy(bt);
        }
        */

        /*
        BobbersList.Clear();
        FishingRodList.Clear();
        LakeList.Clear();
        HookList.Clear();
        FishLineList.Clear();
        BaitList.Clear();

        for (int i = 0; i < ObjectsOnScroll.Count; i++)
        {
            Destroy(ObjectsOnScroll[i]);
        }


        if (shopKind == SHOPKIND.BOBBER)
        {
            if (Bobbers.Count > 0 && ScrollViewContentInPopUp)
            {
                for (int i = 0; i < Bobbers.Count; i++)
                {
                    GameObject card = (GameObject)Instantiate(ShopCardPrefab, ScrollViewContentInPopUp.transform, false);
                    card.GetComponent<CardScript>().Init(Bobbers[i]);
                    BobbersList.Add(card);
                    ObjectsOnScroll.Add(card);
                }
                UpdateBueydItems(BobbersList, preferences.ItemBobbers);
            }
        }

        if (shopKind == SHOPKIND.FISHROD)
        {
            if (FishinRods.Count > 0 && ScrollViewContentInPopUp)
            {
                for (int i = 0; i < FishinRods.Count; i++)
                {
                    GameObject fishingrodCard = (GameObject)Instantiate(ShopCardPrefab, ScrollViewContentInPopUp, false);
                    fishingrodCard.GetComponent<CardScript>().Init(FishinRods[i]);
                    FishingRodList.Add(fishingrodCard);
                    ObjectsOnScroll.Add(fishingrodCard);
                }
                UpdateBueydItems(FishingRodList, preferences.ItemFishingRods);
            }
        }

        if (shopKind == SHOPKIND.LAKE)
        {
            if (Lakes.Count > 0 && ScrollViewContentInPopUp)
            {
                for (int i = 0; i < Lakes.Count; i++)
                {
                    GameObject LakeCard = (GameObject)Instantiate(ShopCardPrefab, ScrollViewContentInPopUp, false);
                    LakeCard.GetComponent<CardScript>().Init(Lakes[i]);
                    LakeList.Add(LakeCard);
                    ObjectsOnScroll.Add(LakeCard);
                }
                UpdateBueydItems(LakeList, preferences.ItemLakes);
            }
        }

        if (shopKind == SHOPKIND.HOOK)
        {
            if (Hooks.Count > 0 && ScrollViewContentInPopUp)
            {
                for (int i = 0; i < Hooks.Count; i++)
                {
                    GameObject HookCard = (GameObject)Instantiate(ShopCardPrefab, ScrollViewContentInPopUp, false);
                    HookCard.GetComponent<CardScript>().Init(Hooks[i]);
                    HookList.Add(HookCard);
                    ObjectsOnScroll.Add(HookCard);
                }
                UpdateBueydItems(HookList, preferences.ItemHooks);
            }
        }

        if (shopKind == SHOPKIND.FISHLINE)
        {
            if (Fishlines.Count > 0 && ScrollViewContentInPopUp)
            {
                for (int i = 0; i < Fishlines.Count; i++)
                {
                    GameObject FishlineCard = (GameObject)Instantiate(ShopCardPrefab, ScrollViewContentInPopUp, false);
                    FishlineCard.GetComponent<CardScript>().Init(Fishlines[i]);
                    FishLineList.Add(FishlineCard);
                    ObjectsOnScroll.Add(FishlineCard);
                }
                UpdateBueydItems(FishLineList, preferences.ItemFishlines);
            }
        }

        if (shopKind == SHOPKIND.BAIT)
        {
            if (Baits.Count > 0 && ScrollViewContentInPopUp)
            {
                for (int i = 0; i < Baits.Count; i++)
                {
                    GameObject BaitCard = (GameObject)Instantiate(ShopCardPrefab, ScrollViewContentInPopUp, false);
                    BaitCard.GetComponent<CardScript>().Init(Baits[i]);
                    BaitList.Add(BaitCard);
                    ObjectsOnScroll.Add(BaitCard);
                }
                UpdateBuyedBaits(BaitList, preferences.ItemBaits, preferences.BuyedBaits);
            }
        }



        PopUpSelectItem.SetActive(true);
        */
    }


    private void OpenItem()
    {
        SceneManager.LoadScene("ByItemScene");
    }

    public void SetSelectedItemLAKE()
    {
        preferences.selectedKindInShop = SHOPKIND.LAKE;
        preferences.returnToMainScene = false;
        OpenItem();
    }
    public void SetSelectedItemBOBBER()
    {
        preferences.selectedKindInShop = SHOPKIND.BOBBER;
        preferences.returnToMainScene = false;
        OpenItem();
    }

    public void SetSelectedItemFISHLINE()
    {
        preferences.selectedKindInShop = SHOPKIND.FISHLINE;
        preferences.returnToMainScene = false;
        OpenItem();
    }

    public void SetSelectedItemFISHROD()
    {
        preferences.selectedKindInShop = SHOPKIND.FISHROD;
        preferences.returnToMainScene = false;
        OpenItem();
    }

    public void SetSelectedItemHOOK()
    {
        preferences.selectedKindInShop = SHOPKIND.HOOK;
        preferences.returnToMainScene = false;
        OpenItem();
    }

    public void SetSelectedItemBAIT()
    {
        preferences.selectedKindInShop = SHOPKIND.BAIT;
        preferences.returnToMainScene = false;
        OpenItem();
    }

}
