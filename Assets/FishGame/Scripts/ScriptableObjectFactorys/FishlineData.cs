using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Fishline", fileName = "New FishlineCard")]
public class FishlineData : ScriptableObject, ShopItemInterface
{
    [Tooltip("Element ID")]
    [SerializeField] private int _fishlineID;

    [Tooltip("Fishline sprite")]
    [SerializeField] private Sprite _fishlineSprite;

    [Tooltip("Rating")]
    [Range(1, 6)]
    [SerializeField] private int _rating = 1;

    [Tooltip("Fishline name")]
    [SerializeField] private string _fishlineName;

    [Tooltip("Max weight")]
    [SerializeField] private int _maxWeight;

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

    [Tooltip("Color fishline")]
    [SerializeField] private Color _fishlineColor;


    public int GetElementID()
    {
        return _fishlineID;
    }

    public int GetMaxWeight()
    {
        return _maxWeight;
    }

    public Sprite GetElementSprite()
    {
        return _fishlineSprite;
    }

    public int GetElementRating()
    {
        return _rating;
    }

    public string GetElementName()
    {
        return _fishlineName;
    }

    public int GetElementPrice()
    {
        return _price;
    }

    public ShopItemType GetElementType()
    {
        return ShopItemType.FISHLINE;
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

    public Color GetFishlineColor()
    {
        return _fishlineColor;
    }
}
