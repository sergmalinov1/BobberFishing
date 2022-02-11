using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour
{

    public Game gameModel;


    public void SettingsClick()
    {
        SceneManager.LoadScene("SettingsScene", LoadSceneMode.Single);
    }


    public void ShopClick()
    {
        SceneManager.LoadScene("ShopScene", LoadSceneMode.Single);
    }



    public void CatchStatisticClick()
    {
        /*
        if (gameModel != null)
        {
            gameModel.ShowTropheyScreen();
        }
        */

        SceneManager.LoadScene("TropheyScene", LoadSceneMode.Single);
    }

    public void MainClick()
    {
        if (gameModel != null)
        {
            gameModel.MainButtonClick();
        }
    }

}
