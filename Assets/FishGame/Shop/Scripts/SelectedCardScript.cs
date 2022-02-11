using UnityEngine;
using UnityEngine.UI;

public class SelectedCardScript : MonoBehaviour
{
    private int _elementID = -1;

    public TMPro.TMP_Text UnitName;
    public Image UnitRating;
    public Image UnitImage;

    public Sprite SpriteRating1;
    public Sprite SpriteRating2;
    public Sprite SpriteRating3;
    public Sprite SpriteRating4;
    public Sprite SpriteRating5;
    public Sprite SpriteRating6;
    private ShopItemInterface _itemdata;
    private LanguageDictionary _languageDictionary;

    public void Init(ShopItemInterface data)
    {

        if (_languageDictionary == null)
        {
            _languageDictionary = FindObjectOfType<LanguageDictionary>();
        }

        if (data != null)
        {
            _itemdata = data;
            _elementID = data.GetElementID();

            if (UnitImage != null)
            {
                UnitImage.sprite = data.GetElementSprite();
            }


            if (data.GetLanguage() != null)
            {
                UnitName.text = _languageDictionary.GetWord(data.GetLanguage(), data.GetMemo());
            }
            else
            {
                UnitName.text = data.GetElementName();
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
        }
    }


    public void UpdateLanguageText()
    {
        if(_itemdata != null)
        {
            if (_itemdata.GetLanguage() != null)
            {
                UnitName.text = _languageDictionary.GetWord(_itemdata.GetLanguage(), _itemdata.GetMemo());
            }
            else
            {
                UnitName.text = _itemdata.GetElementName();
            }
        }
    }





}




