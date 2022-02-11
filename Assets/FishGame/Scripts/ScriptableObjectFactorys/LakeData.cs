using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Lakes", fileName = "New LakeCard")]
public class LakeData : ScriptableObject, ShopItemInterface
{
    [Tooltip("Element ID")]
    [SerializeField] private int _lakeID;

    [Tooltip("Lake sprite")]
    [SerializeField] private Sprite _lakeSprite;

    [Tooltip("Rating")]
    [Range(1, 6)]
    [SerializeField] private int _rating = 1;

    [Tooltip("Lake name")]
    [SerializeField] private string _lakeName;

    [Tooltip("Memo")]
    [SerializeField] private string _memo;

    [Tooltip("Price")]
    [SerializeField] private int _price;

    [Tooltip("Language data")]
    [SerializeField] private LanguageData _languageData;

    [Tooltip("Prefab for shop")]
    [SerializeField] private GameObject _prefabForShop;

    [Tooltip("Scale for shop")]
    [SerializeField] private float _scaleForShop = 1;

    [Tooltip("Materail for background game")]
    [SerializeField] private Material _material;


    public Material GetMaterialForBackground()
    {
        return _material;
    }

    public string GetElementName()
    {
        return _lakeName;
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
        return _lakeSprite;
    }

    public ShopItemType GetElementType()
    {
        return ShopItemType.LAKE;
    }

    public int GetElementID()
    {
        return _lakeID;
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
