using UnityEngine;
using System.Collections;

namespace AssetPlacement
{
    public class AssetSelectorBuilder : MonoBehaviour
    {
        private Canvas m_buttonPlacementCanvas;

        private PlacementService m_placementService;

        // Use this for initialization
        void Start()
        {

            Build();

        }

        public void Awake()
        {
            m_placementService = GetComponent<PlacementService>();
            m_buttonPlacementCanvas = GetComponent<Canvas>();

        }


        private void Build()
        {
            //float x = -427;
            //float y = -320;

            //float x = 1;
            //float y = 1;

            float x = 0.5f;
            float y = 0.5f;

            for (int i = 0; i < m_placementService.AvailableAssets.Length; i++)
            {
                var asset = m_placementService.AvailableAssets[i];

                if (asset != null)
                {
                    if (asset.Prefab != null)
                    {
                        //create UI for that asset and register a click handler
                        string uiName = asset.Prefab.name;
                        if (asset.Count > 1)
                        {
                            uiName += " x " + asset.Count;
                        }
                        //UnityEngine.GUI.Button(new Rect(50, i * 20, 40, 10), uiName);

                        //WTF do i need a prefab for 

                        GameObject objectHolder = Instantiate<GameObject>(new GameObject());

                        objectHolder.transform.position = new Vector3(50, i * 20, 0);
                        objectHolder.transform.localScale = new Vector3(100, 100, 100);
                        objectHolder.transform.parent = m_buttonPlacementCanvas.transform;


                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x, y, 0);
                        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        cube.transform.parent = m_buttonPlacementCanvas.transform;
                    }
                    else
                    {
                        Debug.LogWarning("Level asset defined without a selected prefab!");
                    }
                }
            }
        }


        private void Cleanup()
        {
        }
    }
}