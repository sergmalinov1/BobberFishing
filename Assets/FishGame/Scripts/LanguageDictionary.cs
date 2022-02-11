using System.Collections.Generic;
using UnityEngine;

public class LanguageDictionary : MonoBehaviour
{

    [SerializeField]
    private LanguageData[] WordsArray;

    private SaveDataObject saveDataObject;

    private static GameObject instance;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);

        saveDataObject = FindObjectOfType<SaveDataObject>();

    }

    public string GetWord(string word)
    {
        string result = "Not found";

        SystemLanguage language = saveDataObject.GetSaveData().language;


        List<LanguageData> Words = new List<LanguageData>(WordsArray);

        foreach (LanguageData dictData in Words)
        {
            if( word.ToUpper() == dictData.English.ToUpper())
            {
                if(language == SystemLanguage.English)
                {
                    result = dictData.English;
                } else if (language == SystemLanguage.Russian)
                {
                    result = dictData.Russian;
                } else if (language == SystemLanguage.Ukrainian)
                {
                    result = dictData.Ukraine;
                }
                return result;
            }
        }

        //Debug.Log("Result = " + result);
        return result;
    }

    public string GetWord(LanguageData data)
    {
        if(data == null) { return "null"; }

        return GetWord(data.English);
    }


    public string GetWord(LanguageData data, string memoText)
    {
        string result = GetWord(data.English);

        if(memoText != null)
        {
            result += " " + memoText;
        }

        return result;
    }


}
