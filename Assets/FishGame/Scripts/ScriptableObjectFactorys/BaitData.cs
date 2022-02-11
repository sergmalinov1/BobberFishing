using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Bait", fileName = "New BaitCard")]
public class BaitData : ScriptableObject, ShopItemInterface
{

    [Tooltip("Element ID")]
    [SerializeField] private int _baitID;

    [Tooltip("Bait sprite")]
    [SerializeField] private Sprite _baitSprite;

    [Tooltip("Rating")]
    [Range(1, 6)]
    [SerializeField] private int _rating = 1;

    [Tooltip("Bait name")]
    [SerializeField] private string _baitName;

    [Tooltip("Memo")]
    [SerializeField] private string _memo;

    [Tooltip("Price")]
    [SerializeField] private int _price;

    [Tooltip("Prefab")]
    [SerializeField] private GameObject _prefab;

    [Tooltip("Language data")]
    [SerializeField] private LanguageData _languageData;

    [Tooltip("Count animations for play")]
    [SerializeField] private int _countAnimations;

    [Tooltip("Prefab for shop")]
    [SerializeField] private GameObject _prefabForShop;

    [Tooltip("Scale for shop")]
    [SerializeField] private float _scaleForShop = 1;

    [Tooltip("Scale for shop item")]
    [SerializeField] private float _scaleForShopItem = 1;

    public int GetCountAnimations()
    {
        return _countAnimations;
    }

    public int GetElementID()
    {
        return _baitID;
    }

    public Sprite GetElementSprite()
    {
        return _baitSprite;
    }

    public int GetElementRating()
    {
        return _rating;
    }

    public string GetElementName()
    {
        return _baitName;
    }

    public int GetElementPrice()
    {
        return _price;
    }

    public ShopItemType GetElementType()
    {
        return ShopItemType.BAIT;
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

    public float GetScaleForShopItem()
    {
        return _scaleForShopItem;
    }

}
