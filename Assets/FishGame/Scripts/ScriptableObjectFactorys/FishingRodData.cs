using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Fishingrod", fileName = "New FishingrodCard")]
public class FishingRodData : ScriptableObject, ShopItemInterface
{

    [Tooltip("Element ID")]
    [SerializeField] private int _fishingRodID;

    [Tooltip("Fishingrod sprite")]
    [SerializeField] private Sprite _fishingrodSprite;

    [Tooltip("Rating")]
    [Range(1, 6)]
    [SerializeField] private int _rating = 1;

    [Tooltip("Fishingrod name")]
    [SerializeField] private string _fishingRodName;

    [Tooltip("Memo")]
    [SerializeField] private string _memo;

    [Tooltip("Price")]
    [SerializeField] private int _price;

    [Tooltip("% weight fish (x100)")]
    [Range(0.1f,1f)]
    [SerializeField] private float _percentWeightFish = 0.5f;

    [Tooltip("Language data")]
    [SerializeField] private LanguageData _languageData;

    [Tooltip("Prefab for shop")]
    [SerializeField] private GameObject _prefabForShop;

    [Tooltip("Scale for shop")]
    [SerializeField] private float _scaleForShop = 1;

    public float GetPercentWeight()
    {
        return _percentWeightFish;
    }

    public string GetElementName()
    {
        return _fishingRodName;
    }

    public int GetElementPrice()
    {
        return _price;
    }

    public int GetElementRating()
    {
        return _rating;
    }

    public Sprite GetElementSprite()
    {
        return _fishingrodSprite;
    }

    public ShopItemType GetElementType()
    {
        return ShopItemType.FISHINGROD;
    }

    public int GetElementID()
    {
        return _fishingRodID;
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
