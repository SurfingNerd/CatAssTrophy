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
        public void RestartLevel()
        {
            StaticCatastrophyDataBroker.IsGameStartMode = false;
            RunLevel();
            // Save game data

            // Close game
            
        }

        private void RunLevel()
        {
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
            if (!StaticCatastrophyDataBroker.IsGameStartMode)
            {
                //if we are allready in startmode, than there is allready the most up to date information stored.
                //we would override it with wrong information.
                StorePositions();
            }
                
            StaticCatastrophyDataBroker.IsGameStartMode = true;
            RunLevel();
        }

        private void StorePositions()
        {
            AssetPlacement.PlacementService2D service = FindObjectOfType<AssetPlacement.PlacementService2D>();
            AssetPlacement.StaticCatastrophyDataBroker.StoreLocationPlacementInfo(service.GetPositionInfos());
        }

        //private void ApplyPositions(List<PrefabPositionInfo> positions)
        //{
        //    AssetPlacement.PlacementService2D service = FindObjectOfType<AssetPlacement.PlacementService2D>();
        //    service.ApplyPositions(positions);
        //}
    }
}