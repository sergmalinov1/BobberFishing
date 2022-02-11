using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish", fileName = "New Fish")]
public class FishData : ScriptableObject
{
    [Tooltip("Element ID")]
    [SerializeField] private int _fishID;

    [Tooltip("Fish name")]
    [SerializeField] private string _fishName;

    [Tooltip("Min weight (gr)")]
    [SerializeField] private float _minWeight;

    [Tooltip("Max weight (gr)")]
    [SerializeField] private float _maxWeight;

    [Tooltip("Trophey black photo")]
    [SerializeField] private Sprite _tropheyBlackPhoto;

    [Tooltip("Trophey color photo")]
    [SerializeField] private Sprite _tropheyColorPhoto;

    [Tooltip("Fish object on scene")]
    [SerializeField] private GameObject _fishObject;

    [Tooltip("Live places")]
    [SerializeField] private List<LakeData> _lakesLive;

    [Tooltip("Food")]
    [SerializeField] private List<BaitData> _baites;

    [Tooltip("Price")]
    [SerializeField] private float _price;

    [Tooltip("Language data")]
    [SerializeField] private LanguageData _languageData;


    public float Price
    {
        get { return _price; }
    }


    public int FishID
    {
        get { return _fishID; }
    }

    public string FishName
    {
        get { return _fishName; }
    }

    public float MinWeight
    {
        get { return _minWeight; }
    }
    public float MaxWeight
    {
        get { return _maxWeight; }
    }
    public Sprite TropheyBlackPhoto
    {
        get { return _tropheyBlackPhoto; }
    }
    public Sprite TropheyColorPhoto
    {
        get { return _tropheyColorPhoto; }
    }
    public GameObject FishObject
    {
        get { return _fishObject; }
    }
    public List<LakeData> LakesLive
    {
        get { return _lakesLive; }
    }
    public List<BaitData> Baites
    {
        get { return _baites; }
    }

    public LanguageData GetLanguage()
    {
        return _languageData;
    }
}
