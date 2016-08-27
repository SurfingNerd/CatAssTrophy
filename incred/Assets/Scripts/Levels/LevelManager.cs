using UnityEngine;
using System.Collections;

namespace Levels
{
    public static class LevelManager
    {
        /// <summary>
        /// 0 if we are not in a real level, like start screen or level picker.
        /// </summary>
        public static int CurrentLevel;

        public static readonly int MaxLevels = 5;

        public static void LoadStartScreen()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("0-startScreen");
        }

        public static void LoadLevelPicker()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelPicker");
        }

        public static void LoadLevel(int levelNumber)
        {
            //todo: max levels ?
            CurrentLevel = levelNumber;
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelNumber.ToString() + "-level");
        }

        public static void LoadNextLevel()
        {
            int level = CurrentLevel + 1;
            if (level <= MaxLevels)
            {
                LoadLevel(level);
            }
            else
            {
                LoadLevelPicker();
            }

        }
    }

}