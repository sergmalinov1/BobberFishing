using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Bobbers", fileName = "New bobberCard")]
public class BobberData : ScriptableObject,ShopItemInterface
{

    [Tooltip("Element ID")]
    [SerializeField] private int _bobberID;

    [Tooltip("Bobber sprite")]
    [SerializeField] private Sprite _bobberSprite;


    [Tooltip("Rating")]
    [Range(1, 6)]
    [SerializeField] private int _rating = 1;


    [Tooltip("Bobber name")]
    [SerializeField] private string _bobberName;


    [Tooltip("Memo")]
    [SerializeField] private string _memo;

    [Tooltip("Price")]
    [SerializeField] private int _price;

    [Tooltip("Prefab")]
    [SerializeField] private GameObject _prefab;

    [Tooltip("Prefab for shop")]
    [SerializeField] private GameObject _prefabForShop;

    [Tooltip("Language data")]
    [SerializeField] private LanguageData _languageData;

    [Tooltip("Scale for shop")]
    [SerializeField] private float _scaleForShop = 1;

    public int GetElementID()
    {
        return _bobberID;
    }

    public Sprite GetElementSprite()
    {
        return _bobberSprite;
    }

    public int GetElementRating()
    {
        return _rating;
    }

    public string GetElementName()
    {
        return _bobberName;
    }

    public int GetElementPrice()
    {
        return _price;
    }

    public ShopItemType GetElementType()
    {
        return ShopItemType.BOBBER;
    }


    public GameObject GetPrefab()
    {
        return _prefab;
    }

    public LanguageData GetLanguage()
    {
        return _languageData;
    }

    public string GetMemo()
    {
        return _memo;
    }

    public GameObject GetPrefabForShop()
    {
        return _prefabForShop;
    }

    public float GetScaleForShop()
    {
        return _scaleForShop;
    }
}
