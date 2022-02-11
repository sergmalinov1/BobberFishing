using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveDataObject : MonoBehaviour
{

    private string saveDataFilefName = "savedData.save";
    private SaveData savedData;


    private static GameObject instance;

    private void Awake()
    {
        Start2();
    }

    void Start2()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = gameObject;
        } else
        {
            Destroy(gameObject);
        }

        InitSavedData();
    }

    public SaveData GetSaveData()
    {
        return savedData;
    }

    private void InitSavedData()
    {
        //DebugText.text = "InitSavedData()";
        try
        {
            //Debug.Log("Path = " + Application.persistentDataPath + "/" + saveDataFilefName);
            if (File.Exists(Application.persistentDataPath + "/" + saveDataFilefName))
            {
                //Debug.Log("есть файл");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + saveDataFilefName, FileMode.Open);
                savedData = (SaveData)binaryFormatter.Deserialize(file);

                //Debug.Log("-------рыбы---------------");
                //Debug.Log("Количество = " + savedData.TropheyInfo.Count);

                //Debug
                for (int i = 0; i < savedData.TropheyInfo.Count; i++)
                {
                    List<float> currentElement = savedData.TropheyInfo[i];
                    //string logString = $"Рыба:{currentElement[0]} вес:{currentElement[1]} сумма: {currentElement[2]} ";
                    //Debug.Log(logString);
                }

                //Debug.Log("-------рыбы---------------");
                //DebugText.text += " Успешно";

                file.Close();
                return;
            } else
            {
                InitDefaultSaveData();
                return;
            }
        }
        catch (IOException e)
        {
            //DebugText.text += " Ошибка" + "\n" + e.Message;
            //Debug.Log(e.Message);
            Debug.Log("нет файла");
            Debug.Log(e.Message);
            InitDefaultSaveData();
            return;
        }
    }

    private void InitDefaultSaveData()
    {
        Debug.Log("------------InitDefaultSaveData----------------");
        savedData = new SaveData();
        savedData.ItemBobbers.Add(1);
        savedData.ItemFishingRods.Add(1);
        savedData.ItemFishlines.Add(1);
        savedData.ItemLakes.Add(1);
        savedData.ItemHooks.Add(1);
        savedData.ItemBaits.Add(1);

        List<int> tmpBuyedbaits = new List<int>();
        tmpBuyedbaits.Add(1);
        tmpBuyedbaits.Add(10);
        savedData.BuyedBaits.Add(tmpBuyedbaits);

        savedData.m_Score = 10;

        //savedData.TropheyInfo.Add()

        //DebugText.text += "\n" + "Дефолтные значение";

        saveGameData(true);
    }


    public void saveGameData(bool createFile = false)
    {
        try
        {
            FileStream file;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (createFile)
            {
                file = File.Create(Application.persistentDataPath + "/" + saveDataFilefName);
            }
            else
            {
                file = File.Open(Application.persistentDataPath + "/" + saveDataFilefName, FileMode.OpenOrCreate, FileAccess.Write);
            }

            binaryFormatter.Serialize(file, savedData);
            file.Close();
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
            //DebugText.text += "\n" + "saveGameData()  ошибка " +  "\n" + e.Message;
        }
    }

}
