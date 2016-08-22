using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Levels;

namespace Gui
{
    public class buttonClick : MonoBehaviour
    {
        bool started = false;
        Vector3 startVector;

        public void RestartLevel()
        {
            // Save game data

            // Close game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        private void StartPlacementService()
        {
            AssetPlacement.PlacementService2D placementService = GameObject.FindObjectOfType<AssetPlacement.PlacementService2D>();
            if (placementService != null)
            {
                placementService.StartGame();
            }
        }

        private void StartStartableGameObjects()
        {
            Object[] objs = GameObject.FindObjectsOfType(typeof(GameObject));

            foreach (var obj in objs)
            {
                if (obj is GameObject)
                {
                    GameObject go = obj as GameObject;
                    var startableObjects = go.GetComponents<IStartableGameObject>();
                    foreach (IStartableGameObject startableObject in startableObjects)
                    {
                        startableObject.StartGame();
                    }
                }
            }

        }

        public void startLevel()
        {

            if (!started)
            {
                // Save game data
                GameObject cat = GameObject.Find("coolCat");
                startVector = cat.transform.position;
                cat.GetComponent<Rigidbody2D>().isKinematic = false;
                started = true;
                StartPlacementService();
                StartStartableGameObjects();
            }
            else
            {

                GameObject cat = GameObject.Find("coolCat");
                cat.transform.position = startVector;

            }
        }
    }
}