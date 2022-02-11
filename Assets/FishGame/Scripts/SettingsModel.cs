using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsModel : MonoBehaviour
{

    public Toggle ToggleUSA;
    public Toggle ToggleRU;
    public Toggle ToggleUA;

    private SaveData preferences;

    private SaveDataObject saveDataObject;


    private Donate _donate;

    // Start is called before the first frame update
    void Start()
    {
        saveDataObject = FindObjectOfType<SaveDataObject>();
        if (saveDataObject)
        {
            preferences = saveDataObject.GetSaveData();
        } else
        {
            return;
        }

        UpdateFlagLanguage();
    }


    private void UpdateFlagLanguage()
    {
        if (preferences != null)
        {
            if (preferences.language == SystemLanguage.English)
            {
                ToggleUSA.isOn = true;
            }
            else if (preferences.language == SystemLanguage.Russian)
            {
                ToggleRU.isOn = true;
            }
            else if (preferences.language == SystemLanguage.Ukrainian)
            {
                ToggleUA.isOn = true;
            }
            else
            {
                ToggleUSA.isOn = true;
            }

        }
    }

    public void SetLanguage(string Language)
    {
        if (Language == "en")
        {
            preferences.language = SystemLanguage.English;
        }

        if (Language == "ru")
        {
            preferences.language = SystemLanguage.Russian;
        }

        if (Language == "ua")
        {
            preferences.language = SystemLanguage.Ukrainian;
        }

        saveDataObject.saveGameData();
    }



    public void ButtonBackOnClick()
    {
        SceneManager.LoadScene("mainScene");
    }


}
