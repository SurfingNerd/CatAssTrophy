using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssetPlacement
{
    public class LevelAssetService : MonoBehaviour
    {


        public int AssetCount = 1;
        public LevelAsset[] Assets = new LevelAsset[1];

        //public List<LevelAsset> Assets2 = new List<LevelAsset>();

        //LevelAsset AssetTest = new LevelAsset();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnValidate()
        {
            LevelAsset[] oldAssets = Assets;

            Assets = new LevelAsset[AssetCount];

            int j = 0;
            for (int i = 0; i < oldAssets.Length; i++)
            {
                LevelAsset old = oldAssets[i];
                if (old.Prefab != null)
                {
                    Assets[j++] = old;
                }
            }
        }
    }

    [System.Serializable]
    public class LevelAsset
    {
        
        public GameObject Prefab;
        public int Count = 1;
    }
}