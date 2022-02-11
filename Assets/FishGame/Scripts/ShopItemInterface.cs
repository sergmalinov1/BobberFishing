using UnityEngine;

public interface ShopItemInterface
{
    public int GetElementID();
    public Sprite GetElementSprite();

    public int GetElementRating();

    public string GetElementName();

    public int GetElementPrice();

    public ShopItemType GetElementType();

    public LanguageData GetLanguage();

    public string GetMemo();

    public GameObject GetPrefabForShop();

    public float GetScaleForShop();
}
