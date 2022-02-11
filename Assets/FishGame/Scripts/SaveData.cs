using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int   m_Score = 0;

    public int   BobberID = 1;
    public int   FishingrodID = 1;
    public int   LakeID = 1;
    public int   HookID = 1;
    public int   FishLineID = 1;
    public int   BaitID = 1;

    public float WeightTench = 0f;
    public float WeightCarp = 0f;
    public float WeightBream = 0f;
    public float WeightSlvBeaming = 0f;
    public float WeightRudd = 0f;
    public float WeightRoach = 0f;
    public float WeightCrucian = 0f;

    public bool isTenchOpen = false;
    public bool isCarpOpen = false;
    public bool isBreamOpen = false;
    public bool isBeamingOpen = false;
    public bool isRuddOpen = false;
    public bool isRoachOpen = false;
    public bool isCrucianOpen = false;

    public List<int> ItemBobbers = new List<int>();
    public List<int> ItemFishingRods = new List<int>();
    public List<int> ItemLakes = new List<int>();
    public List<int> ItemHooks = new List<int>();
    public List<int> ItemFishlines = new List<int>();
    public List<int> ItemBaits = new List<int>();

    public List<List<int>> BuyedBaits = new List<List<int>>();
    public List<List<float>> TropheyInfo = new List<List<float>>();

    public SystemLanguage language = SystemLanguage.English;
    public SHOPKIND selectedKindInShop = SHOPKIND.BOBBER;
    public bool returnToMainScene = false;

    public string ToJsonn()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }

}

