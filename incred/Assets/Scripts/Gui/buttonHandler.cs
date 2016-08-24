using UnityEngine;
using System.Collections;
using Levels;

namespace Gui
{
    public class buttonHandler : MonoBehaviour
    {
        public void LoadLevel(int levelNumber)
        {
            LevelManager.LoadLevel(levelNumber);
        }

    }
}
