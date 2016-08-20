using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{ 
    public static void LoadLevel(int levelNumber)
    {

        if (levelNumber == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("0-startScreen");
        }
        else if (levelNumber == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("1-levelPicker");
        }
        else
        {
            //todo: max levels ?
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelNumber.ToString() + "-level");
        }
    }


}
