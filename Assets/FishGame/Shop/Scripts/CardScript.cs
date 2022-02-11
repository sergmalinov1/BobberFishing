using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    private int _elementID = -1;
    private int  _qtyAvailableValue = -1;
    private ShopModelNew _shopModel;

    public TMPro.TMP_Text UnitName;
    public TMPro.TMP_Text UnitPrice;
    public TMPro.TMP_Text ActionText;
    public TMPro.TMP_Text QtyAvailable;
    public Image UnitRating;
    public Image UnitImage;

    public LanguageData ActionLanguageBuy;
    public LanguageData ActionLanguageTake;
    public LanguageData AvailableLanguageTake;

    public Image UnitAction;
    public Image UnitMoney;
    public Image UnitPriceImage;
    public Image BackGround;

    public Sprite SpriteButtonBuy;
    public Sprite SpriteButtonBuyed;

    public Sprite SpriteRating1;
    public Sprite SpriteRating2;
    public Sprite SpriteRating3;
    public Sprite SpriteRating4;
    public Sprite SpriteRating5;
    public Sprite SpriteRating6;

    private bool _isBuyed = false;
    private ShopItemInterface _itemData;

    private LanguageDictionary _languageDictionary;

    public void SetBuyed(bool isBuyed)
    {
        _isBuyed = isBuyed;
        updateBuyedState();
    }


    public void SetQtyAvailable(int qty)
    {
        _qtyAvailableValue = qty;
        if (QtyAvailable)
        {
            QtyAvailable.text = _languageDictionary.GetWord(AvailableLanguageTake) + ":" + qty;
        }
    }

    void Start()
    {
        _shopModel = FindObjectOfType<ShopModelNew>();
        if (_languageDictionary == null)
        {
            _languageDictionary = FindObjectOfType<LanguageDictionary>();
        }
    }


    public void UpdateLanguageText()
    {
        if (_qtyAvailableValue >= 0)
        {
            SetQtyAvailable(_qtyAvailableValue);
        }


        if (_itemData.GetLanguage() != null)
        {
            UnitName.text = _languageDictionary.GetWord(_itemData.GetLanguage(), _itemData.GetMemo());
        }
        else
        {
            UnitName.text = _itemData.GetElementName();
        }
        updateBuyedState();
    }

    public int GetID()
    {
        return _elementID;
    }

    public void Init(ShopItemInterface data)
    {
        if(_languageDictionary == null)
        {
            _languageDictionary = FindObjectOfType<LanguageDictionary>();
        }

        if (QtyAvailable)
        {
            QtyAvailable.text = "";
        }

        if (data != null)
        {
            _elementID = data.GetElementID();
            _itemData = data;

            if (UnitImage != null)
            {
                UnitImage.sprite = data.GetElementSprite();
            }

            if (UnitName != null)
            {
                 if(data.GetLanguage() != null)
                {
                    UnitName.text = _languageDictionary.GetWord(data.GetLanguage(),data.GetMemo());
                } else
                {
                    UnitName.text = data.GetElementName();
                }
            }


            if (UnitPrice != null)
            {
                UnitPrice.text = data.GetElementPrice().ToString();
            }

            if (UnitRating != null)
            {
                switch (data.GetElementRating())
                {
                    case 1:
                        if (SpriteRating1 != null)
                        {
                            UnitRating.sprite = SpriteRating1;
                        }
                        break;

                    case 2:
                        if (SpriteRating2 != null)
                        {
                            UnitRating.sprite = SpriteRating2;
                        }
                        break;

                    case 3:
                        if (SpriteRating3 != null)
                        {
                            UnitRating.sprite = SpriteRating3;
                        }
                        break;

                    case 4:
                        if (SpriteRating4 != null)
                        {
                            UnitRating.sprite = SpriteRating4;
                        }
                        break;

                    case 5:
                        if (SpriteRating5 != null)
                        {
                            UnitRating.sprite = SpriteRating5;
                        }
                        break;
                    case 6:
                        if (SpriteRating6 != null)
                        {
                            UnitRating.sprite = SpriteRating6;
                        }
                        break;
                }
            }
            updateBuyedState();
        }
    }


    private void updateBuyedState()
    {



        if (_itemData.GetElementType() == ShopItemType.BAIT)
        {
            if (BackGround)
            {
                if (_isBuyed)
                {
                    BackGround.color = Color.white;
                }
                else
                {
                    BackGround.color = Color.gray;
                }
            }

            ActionText.text = _languageDictionary.GetWord(ActionLanguageBuy);
            ActionText.color = new Color(255, 200, 0);

        }
        else
        {
            if (UnitMoney != null)
            {
                UnitMoney.gameObject.SetActive(!_isBuyed);
            }

            if (UnitPriceImage != null)
            {
                UnitPriceImage.gameObject.SetActive(!_isBuyed);
            }

            if (UnitAction != null)
            {
                if (_isBuyed)
                {
                    UnitAction.sprite = SpriteButtonBuyed;
                }
                else
                {
                    UnitAction.sprite = SpriteButtonBuy;
                }
            }

            if (ActionText != null)
            {

                if (_isBuyed)
                {
                    ActionText.text = _languageDictionary.GetWord(ActionLanguageTake);
                    ActionText.color = new Color(230, 46, 77);
                }
                else
                {
                    ActionText.text = _languageDictionary.GetWord(ActionLanguageBuy);
                    ActionText.color = new Color(255, 200, 0);
                }
            }
        }
    }



    public void OnClick()
    {
        if (_shopModel != null)
        {
            if (_isBuyed)
            {
                if(_itemData.GetElementType() == ShopItemType.BAIT)
                {
                    _shopModel.BuyElement(_itemData);
                } else
                {
                    _shopModel.ActivateElement(_itemData);
                }
            }
            else
            {
                _shopModel.BuyElement(_itemData);
            }
        }
    }

    public void SelectCard()
    {
        if (_shopModel != null)
        {
            if (_isBuyed)
            {
                if (_itemData.GetElementType() == ShopItemType.BAIT)
                {
                    _shopModel.ActivateElement(_itemData);
                }
            }
        }
    }

}




