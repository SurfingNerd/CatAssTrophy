using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Levels;
using AssetPlacement;
using System;

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

        public void GoToLevelPicker()
        {
            LevelManager.LoadLevelPicker();
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
            UnityEngine.Object[] objs = GameObject.FindObjectsOfType(typeof(GameObject));

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
                StorePositions();
                RestartLevel();

                // Save game data
                GameObject cat = GameObject.Find("coolCat");
                startVector = cat.transform.position;
                cat.GetComponent<Rigidbody2D>().isKinematic = false;
                started = true;
                //ApplyPositions(savedPositions);
                StartPlacementService();
                StartStartableGameObjects();
            }
            else
            {

                GameObject cat = GameObject.Find("coolCat");
                cat.transform.position = startVector;

            }
        }

        private void StorePositions()
        {
            AssetPlacement.PlacementService2D service = FindObjectOfType<AssetPlacement.PlacementService2D>();
            AssetPlacement.StaticLocationPlacementStorage.StoreLocationPlacementInfo(service.GetPositionInfos());
        }

        //private void ApplyPositions(List<PrefabPositionInfo> positions)
        //{
        //    AssetPlacement.PlacementService2D service = FindObjectOfType<AssetPlacement.PlacementService2D>();
        //    service.ApplyPositions(positions);
        //}
    }
}