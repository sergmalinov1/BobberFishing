using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesObjectsDictionary : MonoBehaviour
{

    [SerializeField]
    private List<BobberData> Bobbers;

    [SerializeField]
    private List<FishingRodData> FishinRods;

    [SerializeField]
    private List<LakeData> Lakes;

    [SerializeField]
    private List<FishlineData> Fishlines;

    [SerializeField]
    private List<HookData> Hooks;

    [SerializeField]
    private List<BaitData> Baits;

    [SerializeField]
    public List<FishData> FishListForGame;

    private static GameObject instance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public List<FishData> GetFishList()
    {
        return FishListForGame;
    }

    public List<BobberData> GetBobbersList()
    {
        return Bobbers;
    }
    public List<FishingRodData> GetFishinRodsList()
    {
        return FishinRods;
    }
    public List<LakeData> GetLakesList()
    {
        return Lakes;
    }
    public List<FishlineData> GetFishlinesList()
    {
        return Fishlines;
    }
    public List<HookData> GetHooksList()
    {
        return Hooks;
    }
    public List<BaitData> GetBaitsList()
    {
        return Baits;
    }


}
