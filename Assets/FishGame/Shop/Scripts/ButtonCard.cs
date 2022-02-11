using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCard : MonoBehaviour
{
    [SerializeField] private Image Border;
    [SerializeField] private Transform ItemSpawn;
    [SerializeField] private Image ForegroundImage;
    [SerializeField] private TMP_Text BaitCountText;
    [SerializeField] private Sprite Star1;
    [SerializeField] private Sprite Star2;
    [SerializeField] private Sprite Star3;
    [SerializeField] private Sprite Star4;
    [SerializeField] private Sprite Star5;
    [SerializeField] private Sprite Star6;

    private int _StarsCount = 1;
    private GameObject currentPrefab;

    public void Init(ShopItemInterface item)
    {
        _StarsCount = item.GetElementRating();
        if (currentPrefab)
        {
            Destroy(currentPrefab);
        }

        if (item.GetElementType() == ShopItemType.LAKE)
        {
            Sprite LakeSprite = item.GetElementSprite();
            if (LakeSprite)
            {
                ForegroundImage.sprite = LakeSprite;
            }
        } else
        {
            GameObject prefab = item.GetPrefabForShop();
            if(prefab)
            {
                currentPrefab = Instantiate(prefab, ItemSpawn.position, ItemSpawn.rotation,ItemSpawn);
                currentPrefab.layer = LayerMask.NameToLayer("UI");
                foreach (Transform trans in currentPrefab.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("UI");
                }
                currentPrefab.transform.localScale = Vector3.one * item.GetScaleForShop();
            }
        }
        ReDrawStars();
    }
    

    private void ReDrawStars()
    {
        if(_StarsCount == 1)
        {
            Border.sprite = Star1;
        }

        if (_StarsCount == 2)
        {
            Border.sprite = Star2;
        }

        if (_StarsCount == 3)
        {
            Border.sprite = Star3;
        }

        if (_StarsCount == 4)
        {
            Border.sprite = Star4;
        }

        if (_StarsCount == 5)
        {
            Border.sprite = Star5;
        }

        if (_StarsCount == 6)
        {
            Border.sprite = Star6;
        }
    }

   public void SetQtyAvailable(int qty)
    {
        BaitCountText.text = qty.ToString();
    }
}
