using UnityEngine;
using UnityEngine.UI;

public class PopUpShop : MonoBehaviour
{
    private int _elementID = -1;

    public TMPro.TMP_Text UnitName;
    public TMPro.TMP_Text UnitPrice;
    public Image UnitRating;
    public Image UnitImage;

    public Sprite SpriteRating1;
    public Sprite SpriteRating2;
    public Sprite SpriteRating3;
    public Sprite SpriteRating4;
    public Sprite SpriteRating5;
    public Sprite SpriteRating6;


    private LanguageDictionary _languageDictionary;

    private void Start()
    {

        if (_languageDictionary == null)
        {
            _languageDictionary = FindObjectOfType<LanguageDictionary>();
        }
    }

    public void Init(ShopItemInterface data)
    {

        if (_languageDictionary == null)
        {
            _languageDictionary = FindObjectOfType<LanguageDictionary>();
        }

        if (data != null)
        {
            _elementID = data.GetElementID();

            if (UnitImage)
            {
                UnitImage.sprite = data.GetElementSprite();
            }

            if (UnitName)
            {
                if(data.GetLanguage())
                {
                    UnitName.text = _languageDictionary.GetWord(data.GetLanguage(),data.GetMemo());
                } else
                {
                    UnitName.text = data.GetElementName();
                }
            }

            if (UnitPrice)
            {
                UnitPrice.text = data.GetElementPrice().ToString();
            }

            if (UnitRating)
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

    public void CloseClick()
    {
        gameObject.SetActive(false);
    }

}
