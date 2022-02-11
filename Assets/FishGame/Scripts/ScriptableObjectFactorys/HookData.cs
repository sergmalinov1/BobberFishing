using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Hooks", fileName = "New HookCard")]
public class HookData : ScriptableObject,ShopItemInterface
{
    [Tooltip("Element ID")]
    [SerializeField] private int _hookID;

    [Tooltip("Hook sprite")]
    [SerializeField] private Sprite _hookSprite;


    [Tooltip("Rating")]
    [Range(1, 6)]
    [SerializeField] private int _rating = 1;


    [Tooltip("Hook name")]
    [SerializeField] private string _hookName;

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

    [Tooltip("Scale for shop item")]
    [SerializeField] private float _scaleForShopItem = 1;

    public int GetElementID()
    {
        return _hookID;
    }

    public Sprite GetElementSprite()
    {
        return _hookSprite;
    }

    public int GetElementRating()
    {
        return _rating;
    }

    public string GetElementName()
    {
        return _hookName;
    }

    public int GetElementPrice()
    {
        return _price;
    }

    public ShopItemType GetElementType()
    {
        return ShopItemType.HOOK;
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
